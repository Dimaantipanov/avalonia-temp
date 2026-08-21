using System;

namespace FenceFactory.Desktop.ViewModels.Panels.Director;

/// <summary>
/// Чистый контейнер параметров для разметки стартовой панели регистрации директора.
/// Содержит только сырые данные для передачи. Логика и сетевой контур здесь запрещены.
/// </summary>
public class DirectorRegistrationPanel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}