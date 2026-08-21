using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Services.Models;
using FenceFactory.Desktop.Services.Network;

namespace FenceFactory.Desktop.ViewModels.States.Director;

/// <summary>
/// Каноническая MVVM-версия управления первоначальной регистрацией персонала завода Директором.
/// </summary>
public partial class DirectorCoreStaffRegistrationViewModel : StateViewModelBase
{
    private readonly StaffNetworkClient _staffNetworkClient;

    public override ApplicationState State => ApplicationState.DirectorCoreStaffRegistration;

    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsManagerSelected))]
    [NotifyPropertyChangedFor(nameof(IsSupplierSelected))]
    [NotifyPropertyChangedFor(nameof(IsMasterSelected))]
    private UserRole _selectedRole = UserRole.Manager;

    public bool IsManagerSelected => SelectedRole == UserRole.Manager;
    public bool IsSupplierSelected => SelectedRole == UserRole.Supplier;
    public bool IsMasterSelected => SelectedRole == UserRole.Master;

    [ObservableProperty] private bool _isBusy;

    // Универсальное свойство статуса и флаг типа сообщения
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasStatus))]
    private string _statusMessage = string.Empty;

    [ObservableProperty] 
    private bool _isStatusAnError;

    public bool HasStatus => !string.IsNullOrEmpty(StatusMessage);

    public DirectorCoreStaffRegistrationViewModel(StaffNetworkClient staffNetworkClient)
    {
        _staffNetworkClient = staffNetworkClient ?? throw new ArgumentNullException(nameof(staffNetworkClient));
    }

    public override void Activate()
    {
        Temp.TempLogger.Log($"[MVVM АКТИВАЦИЯ]: Экран {State} готов к декларативному биндингу!");
    }

    [RelayCommand]
    private void SelectRole(UserRole role)
    {
        SelectedRole = role;
      //  Temp.TempLogger.Log($"[ИНТЕРФЕЙС]: Выбрана роль для регистрации: {SelectedRole}");
    }

    [RelayCommand]
    private void ApplyPreset(UserRole role)
    {
        SelectedRole = role;
        Email = $"{role.ToString().ToLower()}@fencefactory.local";
        Password = "TemporaryPassword123!";
       // Temp.TempLogger.Log($"[ИНТЕРФЕЙС]: Применен пресет автозаполнения для: {role}");
    }

    /// <summary>
    /// Асинхронная команда сохранения сотрудника в PostgreSQL через API.
    /// </summary>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task SaveStaffAsync()
    {
        if (IsBusy) return;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            IsStatusAnError = true;
            StatusMessage = "Заполните рабочий Email и временный пароль.";
            return;
        }

        IsBusy = true;
        StatusMessage = string.Empty;

        try
        {
            Temp.TempLogger.Log($"[БИЗНЕС-ЛОГИКА]: Регистрация сотрудника {Email} ({SelectedRole})...");

            var requestData = new CreateUserRequest(
                Email: Email.Trim(),
                Password: Password,
                Role: (byte)((int)SelectedRole - 1)
            );

            var (success, error) = await _staffNetworkClient.CreateStaffAsync(requestData);

            if (success)
            {
                Temp.TempLogger.Log($"[СЕРВЕР ПОДТВЕРЖДАЕТ]: Сотрудник {Email} успешно добавлен.");
                
                // Устанавливаем статус успеха (рамка станет синей)
                IsStatusAnError = false;
                StatusMessage = $"СОТРУДНИК {Email.ToUpper()} УСПЕШНО ДОБАВЛЕН В БАЗУ ДАННЫХ!";

                // Задержка на чтение строки
                await Task.Delay(3500);

                // Чистим поля формы и скрываем блок
                Email = string.Empty;
                Password = string.Empty;
                StatusMessage = string.Empty;
            }
            else
            {
                IsStatusAnError = true;
                StatusMessage = $"Сервер отклонил запрос: {error}";
            }
        }
        catch (Exception ex)
        {
            IsStatusAnError = true;
            StatusMessage = $"Ошибка сети: {ex.Message}";
            Temp.TempLogger.Log($"[ОШИБКА СЕТИ]: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void NextStep()
    {
        Temp.TempLogger.Log("[НАВИГАЦИЯ]: Переход к экрану управления персоналом...");
        NavigationRequested?.Invoke(ApplicationState.DirectorStaffManagement);
    }

    public override void Deactivate()
    {
        Email = string.Empty;
        Password = string.Empty;
        StatusMessage = string.Empty;
        IsStatusAnError = false;
        base.Deactivate();
    }
}
