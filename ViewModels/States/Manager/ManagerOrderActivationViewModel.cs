using Avalonia.Interactivity;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Base;

namespace FenceFactory.Desktop.ViewModels.States.Manager;

/// <summary>
/// Скрипт состояния: Активация готового ордера менеджером.
/// Экран для запуска оплаченных заказов в производство.
/// </summary>
public partial class ManagerOrderActivationViewModel : StateViewModelBase
{
    public override ApplicationState State => ApplicationState.ManagerOrderActivation;

    private Views.Manager.ManagerMainPanel? _mainPanel;
    private ManagerMenuWireCommutator _menuCommutator = new();

    public override void Activate()
    {
     /*   _mainPanel = FenceFactory.Desktop.Views.ViewFactory.CreateView(this.State) as Views.Manager.ManagerMainPanel;
        if (_mainPanel == null) return;

        if (_mainPanel.Viewport.Children.Count > 0 && _mainPanel.Viewport.Children[0] is Avalonia.Controls.UserControl contentSlice)
        {
            contentSlice.DataContext = this;
        }*/

        // Подключаем коммутатор для управления левой панелью меню
      //  _menuCommutator.Connect(_mainPanel.Menu, this.State, targetState => NavigationRequested?.Invoke(targetState));

      if (_mainPanel is not null)
      {
          FenceFactory.Desktop.Views.Auth.AppShellWindow.Instance.SetContent(_mainPanel);
      }


        Temp.TempLogger.Log($"[СКРИПТ МЕНЕДЖЕРА]: Слайс {this.State} (Активация ордера) успешно выведен на холст!");
    }

    public override void Deactivate()
    {
        // Отключаем коммутатор и вычищаем все подписки левой панели
        _menuCommutator.Disconnect();

        _mainPanel = null;

        Temp.TempLogger.Log($"[СКРИПТ ПОДТВЕРЖДАЕТ]: Панель {this.State} успешно деактивирована и выгружена!");
    }
}