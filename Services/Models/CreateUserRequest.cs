namespace FenceFactory.Desktop.Services.Models;

/// <summary>
/// Параметры для отправки на эндпоинт регистрации Директора или Персонала завода.
/// </summary>
public record CreateUserRequest(string Email, string Password, byte Role);