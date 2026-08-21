using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Services.Network;
using FenceFactory.Desktop.Temp;

namespace FenceFactory.Desktop.ViewModels.States.Auth;

/// <summary>
/// Каноническая MVVM-версия экрана авторизации. Без коммутаторов и ручного инжекта.
/// </summary>
public partial class AuthViewModel : StateViewModelBase
{
    private readonly IdentityNetworkClient _networkClient;

    public override ApplicationState State => ApplicationState.Auth;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

    /// <summary>
    /// Конструктор принимает сетевой клиент для работы с API
    /// </summary>
    public AuthViewModel(IdentityNetworkClient networkClient)
    {
        _networkClient = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
    }

    public override void Activate()
    {
        Temp.TempLogger.Log($"[MVVM АКТИВАЦИЯ]: Экран {State} готов к декларативному биндингу!");
    }

    /// <summary>
    /// Асинхронная команда авторизации. Шлет честный запрос на бэкенд через IdentityNetworkClient.
    /// </summary>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;
        IsBusy = true; // Блокирует весь UI (поля ввода и кнопки) через привязку IsEnabled

        try
        {
            Temp.TempLogger.Log($"[БИЗНЕС-ЛОГИКА]: Инициализация входа для {Email}...");
            
            // Вызов твоего сетевого клиента. Если сервер вернет 400/500 или упадет сеть, сработает catch.
            UserRole role = await _networkClient.LoginAsync(Email, Password);

         

            // Навигация сработает ТОЛЬКО при успешном ответе от базы
            NavigateByRole(role);
        }
        catch (Exception ex)
        {
            ErrorMessage = "Ошибка доступа: Неверные данные или нет связи с сервером.";
            Temp.TempLogger.Log($"[MVVM ОШИБКА АВТОРИЗАЦИИ]: {ex.Message}");
        }
        finally
        {
            IsBusy = false; // Железно разблокирует UI
        }
    }

    /// <summary>
    /// Команда для быстрых пресетов автозаполнения. Берет данные СТРОГО из твоего файла AuthPresets.cs.
    /// </summary>
    [RelayCommand]
    private void ApplyPreset(string role)
    {
        ErrorMessage = string.Empty;
        switch (role)
        {
            case "FirstLaunch":
                Email = AuthPresets.BootstrapEmail;
                Password = AuthPresets.BootstrapPassword;
                break;
            case "Director":
                Email = AuthPresets.DirectorEmail;
                Password = AuthPresets.DirectorPassword;
                break;
            case "Manager":
                Email = AuthPresets.ManagerEmail;
                Password = AuthPresets.ManagerPassword;
                break;
            case "Supplier":
                Email = AuthPresets.SupplierEmail;
                Password = AuthPresets.SupplierPassword;
                break;
            case "Master":
                Email = AuthPresets.MasterEmail;
                Password = AuthPresets.MasterPassword;
                break;
        }
        
        Temp.TempLogger.Log($"[ПРЕСЕТ]: Заполнены учетные данные для роли: {role}");
    }

    /// <summary>
    /// Маршрутизация приложения на основе роли. Теперь работает строго через каноничные события данных.
    /// </summary>
    public void NavigateByRole(UserRole role)
    {
        Temp.TempLogger.Log($"[БИЗНЕС-ЛОГИКА]: Получена подтвержденная роль {role}. Смена состояния...");

        switch (role)
        {
            case UserRole.Bootstrap:
                NavigationRequested?.Invoke(ApplicationState.DirectorRegistration);
                break;

            case UserRole.Admin:
                NavigationRequested?.Invoke(ApplicationState.DirectorCoreStaffRegistration);
                break;

            case UserRole.Manager:
                NavigationRequested?.Invoke(ApplicationState.ManagerMaterialTemplate);
                break;

            case UserRole.Supplier:
                NavigationRequested?.Invoke(ApplicationState.SupplierDeficitView);
                break;

            case UserRole.Master:
                NavigationRequested?.Invoke(ApplicationState.MasterTeamRegistration);
                break;

            default:
                Temp.TempLogger.Log("[БИЗНЕС-ЛОГИКА ОШИБКА]: Попытка маршрутизации по неизвестной роли.");
                break;
        }
    }

    public override void Deactivate()
    {
        Email = string.Empty;
        Password = string.Empty;
        ErrorMessage = string.Empty;
        Temp.TempLogger.Log($"[MVVM ДЕАКТИВАЦИЯ]: Экран {State} успешно очищен!");
    }
}
