namespace FenceFactory.Desktop.ViewModels.Panels.Director;

/// <summary>
/// Чистый контейнер параметров для экрана увольнения сотрудников.
/// Логика, UI-команды и сеть здесь КАТЕГОРИЧЕСКИ ЗАПРЕЩЕНЫ.
/// </summary>
public class DirectorStaffManagementPanel
{
    // Будет хранить Email сотрудника, выбранного для увольнения из списка
    public string SelectedEmployeeEmail { get; set; } = string.Empty;
}