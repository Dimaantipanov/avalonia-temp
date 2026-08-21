using System;
using Avalonia.Input;
using Avalonia.Controls;
using FenceFactory.Desktop.ViewModels;
using Avalonia.Interactivity;

namespace FenceFactory.Desktop.Temp;

/// <summary>
/// Временный отладочный пульт для глобального прокликивания состояний ERP кнопками 1 и 2.
/// Полностью совместим с Avalonia 11.
/// </summary>
public static class TempKeyboardTracker
{
    private static IDisposable? _keyboardHandler;

    public static void Start(MainWindowViewModel mainWindowViewModel)
    {
        if (mainWindowViewModel == null) return;

        // Регистрируем глобальный обработчик нажатия клавиш для всех окон Window
        _keyboardHandler = InputElement.KeyDownEvent.AddClassHandler<Window>((sender, keyArgs) =>
        {
            // Кнопка 1 — вперед
            if (keyArgs.Key == Key.D1 || keyArgs.Key == Key.NumPad1)
            {
                // ХИРУРГИЯ: Работаем напрямую с состоянием, минуя команды VM
                HandleNextState(mainWindowViewModel);
                keyArgs.Handled = true;
            }
            // Кнопка 2 — назад
            else if (keyArgs.Key == Key.D2 || keyArgs.Key == Key.NumPad2)
            {
                // ХИРУРГИЯ: Работаем напрямую с состоянием, минуя команды VM
                HandlePrevState(mainWindowViewModel);
                keyArgs.Handled = true;
            }
        }, RoutingStrategies.Tunnel);
    }

    public static void Stop()
    {
        _keyboardHandler?.Dispose();
        _keyboardHandler = null;
    }

    private static void HandleNextState(MainWindowViewModel vm)
    {
        int maxIndex = (int)FenceFactory.Desktop.Enums.ApplicationState.MasterTeamDeletion;
        int currentIndex = (int)vm.CurrentState;
        int nextIndex = currentIndex >= maxIndex ? 0 : currentIndex + 1;
    
        vm.CurrentState = (FenceFactory.Desktop.Enums.ApplicationState)nextIndex;
    }

    private static void HandlePrevState(MainWindowViewModel vm)
    {
        int maxIndex = (int)FenceFactory.Desktop.Enums.ApplicationState.MasterTeamDeletion;
        int currentIndex = (int)vm.CurrentState;
        int prevIndex = currentIndex <= 0 ? maxIndex : currentIndex - 1;

        vm.CurrentState = (FenceFactory.Desktop.Enums.ApplicationState)prevIndex;
    }
}
