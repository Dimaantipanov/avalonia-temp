using System;
namespace FenceFactory.Desktop.Services.Models.Responses;

public class UserRowModel
{
    public bool IsSelected { get; set; }
    public Guid Id { get; set; } // Добавили Id, чтобы знать, кого выделяем/удаляем
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}