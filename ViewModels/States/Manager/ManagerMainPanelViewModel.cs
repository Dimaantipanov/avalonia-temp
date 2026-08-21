using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace FenceFactory.Desktop.ViewModels.States.Manager;

/// <summary>
/// Каноническая ViewModel управления экраном менеджера ERP-системы завода.
/// </summary>
public partial class ManagerMainPanelViewModel : ObservableObject
{
    // Физическая блокировка UI тонкого клиента на время сетевых транзакций к PostgreSQL
    [ObservableProperty]
    private bool _isBusy;

    // Стейт-навигация: хранит ViewModel текущей активной из 12 вложенных панелей
    [ObservableProperty]
    private ObservableObject? _currentActivePanelViewModel;

    public ManagerMainPanelViewModel()
    {
        // Конструктор остается пустым и AOT-friendly для инициализации через DI или фабрику
    }

    /// <summary>
    /// Каноническая асинхронная команда смены вложенных панелей через паттерн Состояния.
    /// Автоматически биндится в XAML кнопками меню.
    /// </summary>
    [RelayCommand]
    private async Task ChangePanelAsync(ObservableObject targetViewModel)
    {
        if (IsBusy || targetViewModel == null) return;

        IsBusy = true;
        try
        {
            // Место для серверной валидации сессии или быстрой проверки прав доступа перед переключением стейта
            await Task.Yield(); 
            
            CurrentActivePanelViewModel = targetViewModel;
        }
        finally
        {
            IsBusy = false;
        }
    }
}