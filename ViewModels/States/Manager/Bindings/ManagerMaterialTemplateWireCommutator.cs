using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Views.Manager.Parts.Templates;
using FenceFactory.Desktop.Temp;
using FenceFactory.Desktop.ViewModels.States.Manager.Visual;
using FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Base;
using FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Materials.Dispatchers;

namespace FenceFactory.Desktop.ViewModels.States.Manager.Bindings;

public class ManagerMaterialTemplateWireCommutator
{
    private MaterialTemplatePanel? _rootView;
    private ManagerMaterialTemplateViewModel? _viewModel;
    private ManagerTopMenuWireCommutator _topMenuCommutator = new();
    private ManagerMaterialTemplateViewPreparer _viewPreparer = new();
    
    // КАНОН: Диспетчер суб-коммутаторов логики объявлен полем класса
    private MaterialSubCommutatorDispatcher _subCommutatorDispatcher = new();

    public void Connect(MaterialTemplatePanel view, ManagerMaterialTemplateViewModel viewModel)
    {
        _rootView = view;
        _viewModel = viewModel;

        // 1. Подключаем саб-коммутатор верхнего меню вкладок
        _topMenuCommutator.Connect(_rootView.TopMenuControl, MaterialTabType.Sheet, OnTabChanged);

        TempLogger.Log("[ManagerMaterialTemplateWireCommutator] Connected successfully.");
    }

    private void OnTabChanged(MaterialTabType targetTab)
    {
        if (_rootView == null || _viewModel == null) return;

        // 1. Делегируем подмену визуала препареру по канону триады №3
        _viewPreparer.SwitchSubPanel(_rootView.PanelContainer, targetTab, _viewModel);

        TempLogger.Log($"[ManagerMaterialTemplateWireCommutator] Visual sub-panel switched via Preparer to: {targetTab}");

        // 2. ВЫЧИЩЕНО: Запускаем диспетчер напрямую. Внутренний тормоз теперь сидит внутри дочерних коммутаторов
        _subCommutatorDispatcher.SwitchMaterial(targetTab);
    }

    public void Disconnect()
    {
        _topMenuCommutator.Disconnect();
        
        // КАНОН: Очищаем активный дочерний коммутатор при закрытии экрана
        _subCommutatorDispatcher.DisconnectActive();

        _rootView = null;
        _viewModel = null;

        TempLogger.Log("[ManagerMaterialTemplateWireCommutator] Disconnected. References cleared.");
    }
}
