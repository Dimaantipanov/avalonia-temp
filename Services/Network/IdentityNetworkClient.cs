using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FenceFactory.Desktop.Services.Models;

namespace FenceFactory.Desktop.Services.Network;

/// <summary>
/// Сетевой агент для связи с серверной частью FenceFactory Core.
/// </summary>
public class IdentityNetworkClient
{
    private readonly HttpClient _httpClient;
    private readonly NetworkSession _session;

    public IdentityNetworkClient(HttpClient httpClient, NetworkSession session)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    /// <summary>
    /// Первичный Bootstrap-вход по временным учетным данным из Readme с определением роли.
    /// </summary>
    public async Task<UserRole> LoginAsync(string email, string password)
    {
        var requestData = new LoginRequest(email, password);
        _httpClient.DefaultRequestHeaders.Authorization = null;

        try
        {
            using var response = await _httpClient.PostAsJsonAsync("/api/auth/login", requestData);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Temp.TempLogger.Log($"[ОТВЕТ СЕРВЕРА ОШИБКА 400]: {errorContent}");
                response.EnsureSuccessStatusCode();
            }

            // Читаем сырой JSON-ответ сервера
            string rawJson = await response.Content.ReadAsStringAsync();
            // ДОБАВЬ ЭТОТ ЛОГ ДЛЯ ОТЛАДКИ:
           // Temp.TempLogger.Log($"[ОТЛАДКА СЫРОГО ОТВЕТА]: {rawJson}");
            
            // Десериализуем JSON в нашу модель из папки Responses
            var authData = System.Text.Json.JsonSerializer.Deserialize<Models.Responses.AuthResponseData>(rawJson);
            
            // ИСПРАВЛЕНО: Вместо возврата Unknown выбрасываем жесткое исключение
            if (authData == null || string.IsNullOrEmpty(authData.Token))
            {
                throw new InvalidOperationException("Не удалось распарсить структуру ответа авторизации. Токен пуст.");
            }
        
            // Записываем в сессию только ЧИСТЫЙ JWT-токен, очищенный от JSON-обертки
            if (authData.Role == "Bootstrap")
            {
                _session.BootstrapToken = authData.Token;
            }
            else
            {
                _session.AuthToken = authData.Token;
            }

           // Temp.TempLogger.Log($"[СЕРВЕР ПОДТВЕРЖДАЕТ]: Временный JWT-токен успешно извлечен для роли: {authData.Role}");

            // Маппим строковый ответ сервера на наш строгий клиентский энум ролей
            return authData.Role switch
            {
                "Bootstrap" => UserRole.Bootstrap,
                "Admin" => UserRole.Admin,
                "Manager" => UserRole.Manager,
                "Supplier" => UserRole.Supplier,
                "Master" => UserRole.Master,
                _ => throw new InvalidOperationException($"Сервер вернул неизвестную роль: {authData.Role}")
            };
        }
        catch (Exception ex)
        {
            Temp.TempLogger.Log($"[СЕРВЕР ОШИБКА]: Данные: [{email}] [{password}]. Причина: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Метод создания постоянной учетной записи реального Директора.
    /// </summary>
    public async Task<bool> CreateDirectorAsync(CreateUserRequest requestData)
    {
        if (!_session.IsBootstrapActive || string.IsNullOrEmpty(_session.BootstrapToken))
        {
            Temp.TempLogger.Log("[СИСТЕМА БЕЗОПАСНОСТИ]: Отклонено. Попытка вызова создания директора без Bootstrap-токена.");
            throw new InvalidOperationException("Отсутствует временный токен авторизации.");
        }

        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", _session.BootstrapToken);

        try
        {
            // Шлем строго типизированную DTO-модель на сервер
            using var response = await _httpClient.PostAsJsonAsync("/api/admin/create-user", requestData);
            
            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Temp.TempLogger.Log($"[СЕРВЕР ОШИБКА РЕГИСТРАЦИИ]: {errorContent}");
                response.EnsureSuccessStatusCode();
            }

            // Директор создан, сжигаем временный bootstrap-токен
            _session.ClearBootstrap();
            Temp.TempLogger.Log("[СЕРВЕР ПОДТВЕРЖДАЕТ]: Новый Директор успешно создан в PostgreSQL.");
            Temp.TempLogger.Log("[СИСТЕМА БЕЗОПАСНОСТИ]: Временный токен сожжен. Вход по bootstrap@fencefactory.local заблокирован.");
            
            return true;
        }
        catch (Exception ex)
        {
            Temp.TempLogger.Log($"[СЕРВЕР ОШИБКА]: Не удалось создать Директора. Причина: {ex.Message}");
            throw;
        }
    }
}
