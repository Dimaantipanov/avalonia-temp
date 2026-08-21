using System;
using Avalonia.Interactivity;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Views.Manager.Parts.Templates;
using FenceFactory.Desktop.Temp;

namespace FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Base;

public class ManagerTopMenuWireCommutator
{
    private TopMenuContent? _view;
    private Action<MaterialTabType>? _onTabChanged;

    public void Connect(TopMenuContent view, MaterialTabType initialState, Action<MaterialTabType> onTabChanged)
    {
        _view = view;
        _onTabChanged = onTabChanged;

        TempLogger.Log($"[TopMenuCommutator] Connected. Initial tab: {initialState}");

        // 1. Устанавливаем активную вкладку визуально
        _view.UpdateActiveTab(initialState);

        // 2. Включаем доступность кнопок
        _view.SetButtonsEnabled(true);

        // 3. Подписываемся на события кликов напрямую без FindControl
        _view.TabSheet.Click += OnTabClick;
        _view.TabPillar.Click += OnTabClick;
        _view.TabBeam.Click += OnTabClick;
        _view.TabFastener.Click += OnTabClick;
        _view.TabCement.Click += OnTabClick;
        _view.TabInert.Click += OnTabClick;
        // КАНОН: Инициализируем контент стартовой панели при первом запуске
        _onTabChanged.Invoke(initialState);
    }

    private void OnTabClick(object? sender, RoutedEventArgs e)
    {
        if (_view == null || _onTabChanged == null) return;

        // Блокируем UI во избежание спама
        _view.SetButtonsEnabled(false);

        // Определяем выбранную вкладку по нажатой кнопке
        MaterialTabType targetTab = MaterialTabType.Sheet;

        if (sender == _view.TabSheet) targetTab = MaterialTabType.Sheet;
        else if (sender == _view.TabPillar) targetTab = MaterialTabType.Pillar;
        else if (sender == _view.TabBeam) targetTab = MaterialTabType.Beam;
        else if (sender == _view.TabFastener) targetTab = MaterialTabType.Fastener;
        else if (sender == _view.TabCement) targetTab = MaterialTabType.Cement;
        else if (sender == _view.TabInert) targetTab = MaterialTabType.Inert;
        
        

        // Визуально переключаем активный статус и возвращаем доступность кнопкам
        _view.UpdateActiveTab(targetTab);
        _view.SetButtonsEnabled(true);

        TempLogger.Log($"[TopMenuCommutator] Tab switched to: {targetTab}");

        // Оповещаем родительский коммутатор экрана через Action
        _onTabChanged.Invoke(targetTab);
    }

    public void Disconnect()
    {
        if (_view != null)
        {
            _view.TabSheet.Click -= OnTabClick;
            _view.TabPillar.Click -= OnTabClick;
            _view.TabBeam.Click -= OnTabClick;
            _view.TabFastener.Click -= OnTabClick;
            _view.TabCement.Click -= OnTabClick;
            _view.TabInert.Click -= OnTabClick;
        }

        _view = null;
        _onTabChanged = null;

        TempLogger.Log("[TopMenuCommutator] Disconnected. References cleared.");
    }
}
