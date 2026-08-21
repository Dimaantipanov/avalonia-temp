using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Services.Models;
using FenceFactory.Desktop.Services.Network;

namespace FenceFactory.Desktop.ViewModels.States.Director;

/// <summary>
/// Каноническая MVVM-версия экрана регистрации директора.
/// Работает без коммутаторов, препареров и ручного поиска контролов.
/// </summary>
public partial class DirectorRegistrationViewModel : StateViewModelBase
{
    private readonly IdentityNetworkClient _networkClient;

    public override ApplicationState State => ApplicationState.DirectorRegistration;

    // Свойства для декларативного биндинга полей ввода в AXAML
    [ObservableProperty]
    private string _newEmail = string.Empty;

    [ObservableProperty]
    private string _newPassword = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasError))]
    private string _errorMessage = string.Empty;

    // Новое реактивное свойство для вывода сообщения об успешной регистрации
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasSuccess))]
    private string _successMessage = string.Empty;

    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public bool HasSuccess => !string.IsNullOrEmpty(SuccessMessage);

    /// <summary>
    /// Конструктор принимает сетевой client для отправки запросов к API
    /// </summary>
    public DirectorRegistrationViewModel(IdentityNetworkClient networkClient)
    {
        _networkClient = networkClient ?? throw new ArgumentNullException(nameof(networkClient));
    }

    public override void Activate()
    {
        Temp.TempLogger.Log($"[MVVM АКТИВАЦИЯ]: Экран {State} готов к декларативному биндингу. Ждем ввода данных Директора.");
    }

    /// <summary>
    /// Асинхронная команда отправки данных директора на бэкенд: POST /api/admin/create-user
    /// </summary>
    [RelayCommand(AllowConcurrentExecutions = false)]
    private async Task RegisterDirectorAsync()
    {
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;

        // Первичная валидация полей на клиенте
        if (string.IsNullOrWhiteSpace(NewEmail) || string.IsNullOrWhiteSpace(NewPassword))
        {
            ErrorMessage = "Ошибка: Все поля обязательны для заполнения.";
            return;
        }

        if (NewPassword != ConfirmPassword)
        {
            ErrorMessage = "Ошибка: Введенные пароли не совпадают.";
            return;
        }

        IsBusy = true; // Блокирует ввод в форме на время запроса к PostgreSQL

        try
        {
            Temp.TempLogger.Log($"[БИЗНЕС-ЛОГИКА]: Отправка запроса регистрации директора для {NewEmail}...");

            // Формируем строго типизированную DTO модель для API
            var requestData = new CreateUserRequest(NewEmail, NewPassword, (byte)UserRole.Bootstrap);

            // Шлем запрос через наш сетевой агент
            bool success = await _networkClient.CreateDirectorAsync(requestData);

            if (success)
            {
                Temp.TempLogger.Log("[БИЗНЕС-ЛОГИКА]: Директор успешно создан. Запуск таймаута уведомления...");
                
                // Выводим сообщение об успехе на экран
                SuccessMessage = "ДИРЕКТОР УСПЕШНО ЗАРЕГИСТРИРОВАН! ПЕРЕХОД НА ЭКРАН ВХОДА...";
                
               // Даем 4 секунды на чтение статуса перед навигацией
               await Task.Delay(4000);

                Temp.TempLogger.Log("[БИЗНЕС-ЛОГИКА]: Сжигаем временную сессию и возвращаем на экран входа.");
                NavigationRequested?.Invoke(ApplicationState.Auth);
            }
            else
            {
                ErrorMessage = "Ошибка: Сервер отклонил регистрацию Директора.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Ошибка сервера: Не удалось создать учетную запись Директора.";
            Temp.TempLogger.Log($"[MVVM ОШИБКА РЕГИСТРАЦИИ ДИРЕКТОРА]: {ex.Message}");
        }
        finally
        {
            IsBusy = false; // Разблокирует интерфейс
        }
    }

    public override void Deactivate()
    {
        // Каноничное зануление данных формы при уходе с экрана
        NewEmail = string.Empty;
        NewPassword = string.Empty;
        ConfirmPassword = string.Empty;
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;

        Temp.TempLogger.Log($"[MVVM ДЕАКТИВАЦИЯ]: Экран {State} успешно очищен, старые ссылки выгружены!");
    }   

    /// <summary>
    /// Команда для быстрого автозаполнения полей директора из файла пресетов.
    /// </summary>
    [RelayCommand]
    private void ApplyPreset(string role)
    {
        ErrorMessage = string.Empty;
        SuccessMessage = string.Empty;
        if (role == "Director")
        {
            NewEmail = FenceFactory.Desktop.Temp.AuthPresets.DirectorEmail;
            NewPassword = FenceFactory.Desktop.Temp.AuthPresets.DirectorPassword;
            ConfirmPassword = FenceFactory.Desktop.Temp.AuthPresets.DirectorPassword;
        }
        Temp.TempLogger.Log($"[ПРЕСЕТ ДИРЕКТОРА]: Поля успешно заполнены корпоративными данными.");
    }
}
