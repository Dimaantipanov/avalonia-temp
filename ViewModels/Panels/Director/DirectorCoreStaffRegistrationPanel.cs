namespace FenceFactory.Desktop.ViewModels.Panels.Director;

/// <summary>
/// Чистый контейнер параметров для первоначальной регистрации сотрудников.
/// </summary>
public class DirectorCoreStaffRegistrationPanel
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// Числовое значение роли для бэкенда (1 - Manager, 2 - Supplier, 3 - Master и т.д.)
    /// </summary>
    public int SelectedRole { get; set; } = 1; 
}