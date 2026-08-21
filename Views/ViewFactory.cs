using Avalonia.Controls;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Views.Director;
using FenceFactory.Desktop.Views.Manager;
using FenceFactory.Desktop.Views.Manager.Parts;
using FenceFactory.Desktop.Views.Auth;
using FenceFactory.Desktop.Views.Supplier;
using FenceFactory.Desktop.Views.Supplier.Parts;
// Подключаем пространства имен Мастера
using FenceFactory.Desktop.Views.Master;
using FenceFactory.Desktop.Views.Master.Parts;
using FenceFactory.Desktop.Views.Manager.Parts.Templates;
using FenceFactory.Desktop.Views.Manager.Parts.Drafts;
using FenceFactory.Desktop.Views.Manager.Parts.Orders;

namespace FenceFactory.Desktop.Views;

public static class ViewFactory
{
    private static ManagerMainPanel? _cachedManagerWorkspace;
    private static SupplierMainPanel? _cachedSupplierWorkspace;
    private static MasterMainPanel? _cachedMasterWorkspace; // Кэш для Мастера

    public static Control CreateView(ApplicationState state)
    {
        return state switch
        {
            // === СЕКЦИЯ АВТОРИЗАЦИИ ===
            ApplicationState.Auth => new AuthView(),

            // === СЕКЦИЯ ДИРЕКТОРА ===
            ApplicationState.DirectorRegistration => new Director_RegistrationPanel(),
            ApplicationState.DirectorCoreStaffRegistration => new DirectorCoreStaffRegistrationPanel(),

            ApplicationState.DirectorStaffManagement => new Director_StaffManagementPanel(),

            // === СЕКЦИЯ МЕНЕДЖЕРА ===
            ApplicationState.ManagerMaterialTemplate => GetCompiledManagerWorkspace(new MaterialTemplatePanel()),
            ApplicationState.ManagerDraft => GetCompiledManagerWorkspace(new DraftPanel()),
            ApplicationState.ManagerOrderActivation => GetCompiledManagerWorkspace(new OrderPanel()),

            // === СЕКЦИЯ СНАБЖЕНЦА ===
            ApplicationState.SupplierDeficitView => GetCompiledSupplierWorkspace(new SupplierDeficitPanel()),
            ApplicationState.SupplierProcurement => GetCompiledSupplierWorkspace(new SupplierReceiptPanel()),

            // === СЕКЦИЯ МАСТЕРА (Твои 3 новых шикарных слайса) ===
            ApplicationState.MasterTeamRegistration => GetCompiledMasterWorkspace(new MasterTeamRegistrationPanel()),
            ApplicationState.MasterOrderRelease => GetCompiledMasterWorkspace(new MasterOrderReleasePanel()),
            ApplicationState.MasterTeamDeletion => GetCompiledMasterWorkspace(new MasterTeamDeletionPanel()),

            _ => throw new System.ArgumentOutOfRangeException(nameof(state), state, "Неизвестный стейт в фабрике")
        };
    }

    private static Control GetCompiledManagerWorkspace(Control innerContentPanel)
    {
        if (_cachedManagerWorkspace == null) _cachedManagerWorkspace = new ManagerMainPanel();
        var viewport = _cachedManagerWorkspace.FindControl<Grid>("ManagerViewport");
        if (viewport != null)
        {
            viewport.Children.Clear();
            viewport.Children.Add(innerContentPanel);
        }
        return _cachedManagerWorkspace;
    }

    private static Control GetCompiledSupplierWorkspace(Control innerContentPanel)
    {
        if (_cachedSupplierWorkspace == null) _cachedSupplierWorkspace = new SupplierMainPanel();
        var viewport = _cachedSupplierWorkspace.FindControl<Grid>("SupplierViewport");
        if (viewport != null)
        {
            viewport.Children.Clear();
            viewport.Children.Add(innerContentPanel);
        }
        return _cachedSupplierWorkspace;
    }

    /// <summary>
    /// Собирает составной интерфейс Мастера: чистит MasterViewport и силой вшивает новый слайс
    /// </summary>
    private static Control GetCompiledMasterWorkspace(Control innerContentPanel)
    {
        // 1. Создаем каркас Мастера один раз и держим в памяти
        if (_cachedMasterWorkspace == null)
        {
            _cachedMasterWorkspace = new MasterMainPanel();
        }

        // 2. Находим по имени правое поле Grid
        var viewport = _cachedMasterWorkspace.FindControl<Grid>("MasterViewport");
        if (viewport != null)
        {
            // 3. Стерилизуем вьюпорт от старого слайса
            viewport.Children.Clear();
            
            // 4. Вшиваем целевой слайс Мастера
            viewport.Children.Add(innerContentPanel);
        }

        return _cachedMasterWorkspace;
    }

    public static void ClearCache()
    {
        _cachedManagerWorkspace = null;
        _cachedSupplierWorkspace = null;
        _cachedMasterWorkspace = null; // Чистим Мастера при выходе (Clean State)
    }
}
