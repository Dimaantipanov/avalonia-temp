using Avalonia.Interactivity;
using FenceFactory.Desktop.Enums;
using FenceFactory.Client.Manager;
using FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Base;
using FenceFactory.Desktop.ViewModels.States.Manager.Bindings;

namespace FenceFactory.Desktop.ViewModels.States.Manager;

public partial class ManagerMaterialTemplateViewModel : StateViewModelBase
{
    public override ApplicationState State => ApplicationState.ManagerMaterialTemplate;

    public ManagerMaterialPanel Panel { get; private set; } = new();

    private Views.Manager.ManagerMainPanel? _mainPanel;
    private ManagerMenuWireCommutator _menuCommutator = new();
    private ManagerMaterialTemplateWireCommutator _sliceCommutator = new();

    public override void Activate()
    {
     /*   _mainPanel = Views.ViewFactory.CreateView(this.State) as Views.Manager.ManagerMainPanel;
        if (_mainPanel == null) return;

        if (_mainPanel.Viewport.Children.Count > 0 && _mainPanel.Viewport.Children[0] is Views.Manager.Parts.Templates.MaterialTemplatePanel contentSlice)
        {
            contentSlice.DataContext = this;
            
            // Подключаем единый нестатический коммутатор для управления внутренностями слайса
            _sliceCommutator.Connect(contentSlice, this);
        }

        // Подключаем коммутатор для управления левой панелью меню
        _menuCommutator.Connect(_mainPanel.Menu, this.State, targetState => NavigationRequested?.Invoke(targetState));

        Views.Auth.AppShellWindow.Instance.SetContent(_mainPanel);*/
    }

    public override void Deactivate()
    {
        // Отключаем коммутаторы и начисто вычищаем все подписки
        _menuCommutator.Disconnect();
        _sliceCommutator.Disconnect();

        _mainPanel = null;
        Panel = null!;
    }
}