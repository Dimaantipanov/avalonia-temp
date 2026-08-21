using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FenceFactory.Desktop.Services.Models;

namespace FenceFactory.Desktop.Services.Network;

/// <summary>
/// Изолированный сетевой клиент для управления персоналом.
/// Избавляет ViewModel от "грязного" кода работы с HttpClient.
/// </summary>
public class StaffNetworkClient
{
    private readonly HttpClient _httpClient;
    private readonly NetworkSession _session;

    public StaffNetworkClient(HttpClient httpClient, NetworkSession session)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    /// <summary>
    /// Отправляет запрос на создание сотрудника. Возвращает кортеж (Успех, Текст Ошибки).
    /// </summary>
    public async Task<(bool Success, string? Error)> CreateStaffAsync(CreateUserRequest requestData)
    {
        if (string.IsNullOrEmpty(_session.AuthToken))
        {
            return (false, "Критическая ошибка сессии: токен авторизации отсутствует.");
        }

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/admin/create-staff")
        {
            Content = JsonContent.Create(requestData)
        };

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _session.AuthToken);

        using var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        string errorContent = await response.Content.ReadAsStringAsync();
        return (false, errorContent);
    }
}