using System;
using System.Threading.Tasks;
using FenceFactory.Desktop.Services.Network;

namespace FenceFactory.Desktop.ViewModels.Panels.Auth;

/// <summary>
/// Чистый контейнер параметров для разметки стартовой панели.
/// Сохраняет ввод пользователя и по кнопке шлет данные в сетевой слой бэкенда.
/// </summary>
public class MainWindowPanel
{
    private readonly IdentityNetworkClient _networkClient;

    // Свойства для прямой записи параметров из TextBox-ов разметки
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public MainWindowPanel(IdentityNetworkClient networkClient)
    {
        _networkClient = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
    }

    /// <summary>
    /// Метод, жестко привязанный к клику кнопки "Войти"
    /// </summary>
    public async Task ExecuteAuthAsync()
    {
        // TODO: DELETE (Временный трассировщик ввода для тестов)
        Temp.TempLogger.Log($"[DEV] Auth Try: Em='{Email}', Pw='{Password}'");
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            return;
        }

        // Прямой выстрел параметров в серверную часть через клиент
        await _networkClient.LoginAsync(Email, Password);
    }
}