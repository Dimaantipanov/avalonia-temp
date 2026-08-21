using System.Text.Json.Serialization;

namespace FenceFactory.Desktop.Services.Models.Responses;

/// <summary>
/// DTO-контейнер для десериализации успешного ответа авторизации от сервера.
/// </summary>
public class AuthResponseData
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;
}