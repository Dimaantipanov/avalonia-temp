namespace FenceFactory.Desktop.Services.Network;

/// <summary>
/// Хранилище сессии и токенов авторизации ERP-системы FenceFactory.
/// Разделяет одноразовый Bootstrap-доступ и постоянные рабочие сессии ролей.
/// </summary>
public class NetworkSession
{
    /// <summary>
    /// Временный токен для первичной регистрации Директора (срок жизни 15 минут).
    /// </summary>
    public string? BootstrapToken { get; set; }

    /// <summary>
    /// Постоянный токен авторизации текущего пользователя (Директор, Снабженец, Мастер).
    /// </summary>
    public string? AuthToken { get; set; }

    /// <summary>
    /// Флаг, показывающий, находится ли система в режиме холодного старта.
    /// </summary>
    public bool IsBootstrapActive => !string.IsNullOrEmpty(BootstrapToken);

    /// <summary>
    /// Сброс временного токена после успешного создания легитимного Директора (Anti-Fraud).
    /// </summary>
    public void ClearBootstrap()
    {
        BootstrapToken = null;
    }

    /// <summary>
    /// Полный сброс сессии при выходе из системы.
    /// </summary>
    public void Reset()
    {
        BootstrapToken = null;
        AuthToken = null;
    }
}