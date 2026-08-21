using CommunityToolkit.Mvvm.ComponentModel;
using FenceFactory.Desktop.Enums;
using System.Diagnostics;

namespace FenceFactory.Desktop.ViewModels.States;

public abstract partial class StateViewModelBase : ViewModelBase
{
    public abstract ApplicationState State { get; }

    // ВОТ ЭТОЙ СТРОКИ НЕ ХВАТАЛО. Добавь её сюда:
    [ObservableProperty]
    private bool _isBusy;

    public virtual void Activate() { }
    public virtual void Deactivate() { }

    public System.Action<FenceFactory.Desktop.Enums.ApplicationState>? NavigationRequested;
}