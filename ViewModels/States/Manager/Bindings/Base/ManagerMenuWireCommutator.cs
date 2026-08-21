using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FenceFactory.Desktop.Views.Manager.Parts;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Temp;

namespace FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Base;

public class ManagerMenuWireCommutator
{
    private ManagerMenuPart? _menu;
    private Action<ApplicationState>? _onNavigate;
    private ApplicationState _currentState;

    public void Connect(ManagerMenuPart menu, ApplicationState currentState, Action<ApplicationState> onNavigate)
    {
        _menu = menu;
        _currentState = currentState;
        _onNavigate = onNavigate;
        TempLogger.Log($"[WireCommutator] Connect invoked. Current state: {currentState}. Buttons attached and visual state updated.");

        
        // 1. Красим нужную кнопку через твой метод в панели
        _menu.UpdateActiveButton(this._currentState);

        // 2. Все кнопки оставляем Enabled = true, чтобы работал стиль active
        _menu.CreateTemplateButton.IsEnabled = true;
        _menu.FillDraftButton.IsEnabled = true;
        _menu.ActivateOrderButton.IsEnabled = true;
        _menu.ExitButton.IsEnabled = true;

        // 3. Подписываемся на клики
        _menu.CreateTemplateButton.Click += OnCreateTemplateClick;
        _menu.FillDraftButton.Click += OnFillDraftClick;
        _menu.ActivateOrderButton.Click += OnActivateOrderClick;
        _menu.ExitButton.Click += OnExitClick;
    }

    // В кликах проверяем: если это текущий стейт — просто игнорируем нажатие (защита от дублирования)
    private void OnCreateTemplateClick(object? sender, RoutedEventArgs e) 
    {
        if (_currentState == ApplicationState.ManagerMaterialTemplate) return;
        ExecuteNavigation(ApplicationState.ManagerMaterialTemplate);
    }
    private void OnFillDraftClick(object? sender, RoutedEventArgs e) 
    {
        if (_currentState == ApplicationState.ManagerDraft) return;
        ExecuteNavigation(ApplicationState.ManagerDraft);
    }
   
    private void OnActivateOrderClick(object? sender, RoutedEventArgs e) 
    {
        if (_currentState == ApplicationState.ManagerOrderActivation) return;
        ExecuteNavigation(ApplicationState.ManagerOrderActivation);
    }
    private void OnExitClick(object? sender, RoutedEventArgs e) => ExecuteNavigation(ApplicationState.Auth);

    private void ExecuteNavigation(ApplicationState targetState)
    {
        if (_menu == null || _onNavigate == null) return;

        // UI-блокировка от двойных кликов в локальном контуре
        _menu.CreateTemplateButton.IsEnabled = false;
        _menu.FillDraftButton.IsEnabled = false;
        _menu.ActivateOrderButton.IsEnabled = false;
        _menu.ExitButton.IsEnabled = false;

        _onNavigate.Invoke(targetState);
    }

    public void Disconnect()
    {
        if (_menu != null)
        {
            // Начисто срезаем подписки, исключая утечки памяти
            _menu.CreateTemplateButton.Click -= OnCreateTemplateClick;
            _menu.FillDraftButton.Click -= OnFillDraftClick;
            _menu.ActivateOrderButton.Click -= OnActivateOrderClick;
            _menu.ExitButton.Click -= OnExitClick;
        }

        _menu = null;
        _onNavigate = null;
    }
}
