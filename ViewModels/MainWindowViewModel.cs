using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Services.Network; // Добавлено для NetworkSession и IdentityNetworkClient
using FenceFactory.Desktop.ViewModels.States;
using System;
using System.Net.Http; // Добавлено для HttpClient

namespace FenceFactory.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    // Ручной проброс сетевых зависимостей (Pure DI) в единственном экземпляре
    private readonly HttpClient _httpClient;
    private readonly NetworkSession _networkSession;
    private readonly IdentityNetworkClient _identityNetworkClient;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(HeaderText))]
    private ApplicationState _currentState = ApplicationState.Auth;

    [ObservableProperty] private ViewModelBase? _currentPage;

    [ObservableProperty] private string _runtimeLogText = "СИСТЕМА ИНИЦИАЛИЗИРОВАНА. СТАРТ С: Auth";

    public string HeaderText => CurrentState switch
    {
        ApplicationState.Auth => "АВТОРИЗАЦИЯ В СИСТЕМЕ",
        ApplicationState.DirectorRegistration => "РЕГИСТРАЦИЯ ДИРЕКТОРА",
        ApplicationState.ManagerMaterialTemplate => "КОНФИГУРАТОР СПЕЦИФИКАЦИИ SKU",
        ApplicationState.ManagerDraft => "ЗАПОЛНЕНИЕ ЧЕРНОВИКА ЗАКАЗА",
        ApplicationState.ManagerOrderActivation => "АКТИВАЦИЯ ОРДЕРА МЕНЕДЖЕРА",
        _ => "РАБОЧАЯ ОБЛАСТЬ ERP"
    };

    public MainWindowViewModel()
    {
        // Инициализируем сетевую сессию завода в единственном экземпляре
        _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7259/") };
        _networkSession = new NetworkSession();
        _identityNetworkClient = new IdentityNetworkClient(_httpClient, _networkSession);

        // Стартовый запуск
        NavigateToState(ApplicationState.Auth);
    }


    [RelayCommand]
    public void NavigateToState(ApplicationState newState)
    {
        CurrentState = newState;

        // Каноничная деактивация и очистка ссылок для предотвращения утечек памяти
        if (CurrentPage is StateViewModelBase oldState)
        {
            oldState.NavigationRequested = null;
            oldState.Deactivate();
        }

        var createPage = CreateViewModelForState(newState);

        if (createPage is StateViewModelBase newStateScript)
        {
            newStateScript.NavigationRequested = NavigateToState;
            newStateScript.Activate();
        }

        CurrentPage = createPage;
        RuntimeLogText = $"[ПЕРЕКЛЮЧЕНО] Текущий экран: {newState}";
    }

    private ViewModelBase CreateViewModelForState(ApplicationState state) => state switch
    {
        ApplicationState.Auth => new States.Auth.AuthViewModel(_identityNetworkClient),

        ApplicationState.DirectorRegistration => new States.Director.DirectorRegistrationViewModel(
            _identityNetworkClient),

        ApplicationState.DirectorCoreStaffRegistration => new States.Director.DirectorCoreStaffRegistrationViewModel(
            new StaffNetworkClient(_httpClient, _networkSession)),


        ApplicationState.DirectorStaffManagement => new States.Director.DirectorStaffManagementViewModel(_httpClient,
            _networkSession),


        ApplicationState.ManagerMaterialTemplate => new States.Manager.ManagerMaterialTemplateViewModel(),
        ApplicationState.ManagerDraft => new States.Manager.ManagerDraftViewModel(),
        ApplicationState.ManagerOrderActivation => new States.Manager.ManagerOrderActivationViewModel(),
        ApplicationState.SupplierDeficitView => new States.Supplier.SupplierDeficitViewModel(),
        ApplicationState.SupplierProcurement => new States.Supplier.SupplierProcurementViewModel(),
        ApplicationState.MasterTeamRegistration => new States.Master.MasterTeamRegistrationViewModel(),
        ApplicationState.MasterOrderRelease => new States.Master.MasterOrderReleaseViewModel(),
        ApplicationState.MasterTeamDeletion => new States.Master.MasterTeamDeletionViewModel(),
        _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
    };
}