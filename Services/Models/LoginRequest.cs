namespace FenceFactory.Desktop.Services.Models;

/// <summary>
/// Параметры для передачи на эндпоинт авторизации (Bootstrap)
/// </summary>
public record LoginRequest(string Email, string Password);