using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Services.Models.Responses;
using FenceFactory.Desktop.Services.Network;
using FenceFactory.Desktop.ViewModels.States.Director.Parts;

namespace FenceFactory.Desktop.ViewModels.States.Director;

/// <summary>
/// Скрипт состояния: Управление персоналом (Каноничный MVVM).
/// </summary>
public partial class DirectorStaffManagementViewModel : StateViewModelBase
{
    private readonly HttpClient _httpClient;
    private readonly NetworkSession _session;

    public override ApplicationState State => ApplicationState.DirectorStaffManagement;

    [ObservableProperty] private ObservableCollection<EmployeeCardViewModel> _employees = new();

    [ObservableProperty] private bool _isBusy;

    [ObservableProperty] private string _errorMessage = string.Empty;

    [ObservableProperty] private bool _hasError;

    [ObservableProperty] private EmployeeCardViewModel? _selectedEmployee;

    // Внедряем зависимости через конструктор. Их передает вызывающий код/фабрика.
    public DirectorStaffManagementViewModel(HttpClient httpClient, NetworkSession session)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _session = session ?? throw new ArgumentNullException(nameof(session));
    }

    public override async void Activate()
    {
        // Каноничный вызов базового класса вместо кастомных препареров
        base.Activate();
        await LoadEmployeesAsync();
        Temp.TempLogger.Log($"[СКРИПТ ПОДТВЕРЖДАЕТ]: Панель {this.State} активирована.");
    }

    /// <summary>
    /// Асинхронная команда загрузки сотрудников с автоматической блокировкой интерфейса.
    /// </summary>
    [RelayCommand]
    private async Task LoadEmployeesAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        HasError = false;
        ErrorMessage = string.Empty;

        try
        {
            Temp.TempLogger.Log("[СЕТЕВОЙ ПОТОК]: Отправка запроса на бэкенд...");

            // Настраиваем авторизацию из вашей NetworkSession
            _httpClient.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(_session.AuthToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _session.AuthToken);
            }

            var result =
                await _httpClient.GetFromJsonAsync<System.Collections.Generic.List<UserRowModel>>(
                    "/api/admin/get-staff");

            // Чистим старые подписки во избежание утечек памяти
            foreach (var emp in Employees)
            {
                emp.DeleteRequested -= OnEmployeeDeleteRequested;
            }

            Employees.Clear();

            if (result != null)
            {
                foreach (var empModel in result)
                {
                   

                    // Канон Варианта 2: Оборачиваем DTO в дочернюю ViewModel строки
                    var cardVm = new EmployeeCardViewModel(empModel);

                    // Подписываемся на событие удаления, генерируемое строкой
                    cardVm.DeleteRequested += OnEmployeeDeleteRequested;

                    Employees.Add(cardVm);
                }

                Temp.TempLogger.Log($"[БИЗНЕС-ЛОГИКА]: Успешно загружено {Employees.Count} сотрудников.");
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = $"Не удалось загрузить штат: {ex.Message}";
            Temp.TempLogger.Log($"[БИЗНЕС-ЛОГИКА ОШИБКА]: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    /// <summary>
    /// Обработчик события удаления, пришедшего из конкретной строки-карточки.
    /// </summary>
    private async void OnEmployeeDeleteRequested(object? sender, EmployeeCardViewModel cardVm)
    {
        await FireEmployeeAsync(cardVm);
    }

    /// <summary>
    /// Внутренний метод увольнения, принимающий контекст дочерней карточки.
    /// </summary>
    private async Task FireEmployeeAsync(EmployeeCardViewModel? cardVm)
    {
        if (cardVm == null || IsBusy) return;

        IsBusy = true;
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            if (!string.IsNullOrEmpty(_session.AuthToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _session.AuthToken);
            }

            // Достаем ID из внутренней DTO-модели карточки
            using var response = await _httpClient.DeleteAsync($"/api/admin/fire-staff/{cardVm.Model.Id}");
            if (response.IsSuccessStatusCode)
            {
                // Отписываемся от удаляемого элемента перед удалением из коллекции
                cardVm.DeleteRequested -= OnEmployeeDeleteRequested;

                Employees.Remove(cardVm);
                Temp.TempLogger.Log($"[БИЗНЕС-ЛОГИКА]: Сотрудник {cardVm.Model.Id} успешно удален.");
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = $"Ошибка удаления: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    public override void Deactivate()
    {
        // Канон: Обязательная очистка подписок всех элементов при закрытии экрана
        foreach (var emp in Employees)
        {
            emp.DeleteRequested -= OnEmployeeDeleteRequested;
        }

        Employees.Clear();

        Temp.TempLogger.Log($"[СКРИПТ ПОДТВЕРЖДАЕТ]: Панель {this.State} деактивирована!");
        base.Deactivate();
    }
}