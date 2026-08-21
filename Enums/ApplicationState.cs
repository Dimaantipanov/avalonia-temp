namespace FenceFactory.Desktop.Enums;

/// <summary>
/// Все возможные экраны и состояния ERP-системы строго по регламенту FenceFactory
/// </summary>
public enum ApplicationState
{
    // === 7.0.1 ===
    Auth,                         // Окно входа в систему (MainWindow.axaml / AuthView)

    // === СЕКЦИЯ ДИРЕКТОРА ===
    DirectorRegistration,         // 7.0.2 Регистрация Директора (Director_RegistrationPanel)
    DirectorCoreStaffRegistration,// 7.0.3 Регистрация ядра штата (Director_Staff_InitialRoutePanel)
    DirectorStaffManagement,      // 7.0.4 Рабочий стол Директора увольнение (Director_Enterprise_AuditPanel)
   

    // === СЕКЦИЯ МЕНЕДЖЕРА ===
    ManagerMaterialTemplate,      // 7.0.5 Конфигуратор эталонной «Матрицы» материалов
    ManagerDraft,                 // 7.0.6 Черновик ордера
    ManagerOrderActivation,       // 7.0.7 Активация ордера Менеджером

    // === СЕКЦИЯ СНАБЖЕНЦА ===
    SupplierDeficitView,          // 7.0.8 Панель снабженца нехватка материалов
    SupplierProcurement,          // 7.0.9 Оформление прихода от снабженца

    // === СЕКЦИЯ МАСТЕРА ===
    MasterTeamRegistration,       // 7.0.10 Мастер регистрация бригад
    MasterOrderRelease,           // 7.0.11 Мастер получение списка укомплектованных ордеров
    MasterTeamDeletion            // 7.0.12 Мастер удаление бригад
}