using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FenceFactory.Desktop.Services.Models.Responses;
using FenceFactory.Desktop.Services.Network;

namespace FenceFactory.Desktop.ViewModels.States.Director.Parts;

public partial class EmployeeCardViewModel : ObservableObject
{
    public UserRowModel Model { get; }

    public string Email => Model.Email ?? string.Empty;

    /// <summary>
    /// Железный фронтенд-энум для маршрутизации экранов и логики в Avalonia UI.
    /// </summary>
    public UserRole SystemRole { get; }

    /// <summary>
    /// Человеческий вывод должностей на экран панели для Директора.
    /// </summary>
    public string RolePrefix
    {
        get
        {
            switch (Model.Role?.Trim())
            {
                case "Admin": return "Директор завода";
                case "Manager": return "Менеджер по продажам";
                case "Supplier": return "Снабженец производства";
                case "Master": return "Мастер цеха";
                default: return $"Неизвестная роль #{Model.Role}";
            }
        }
    }


    public event EventHandler<EmployeeCardViewModel>? DeleteRequested;

    public EmployeeCardViewModel(UserRowModel model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));

        // НАШ ТЕСТ: Отправляем точные данные в твою панель логов.
        // Замени "ТвойКлассЛоггера.Log" на реальное имя твоего статического логгера в проекте
        Temp.TempLogger.Log(
            $"[ОТЛАДКА КАРТОЧКИ] Email: {model.Email} | Сырая Роль: {model.Role} | Тип поля: {model.Role?.GetType().Name ?? "null"}");

        // Переводим числовую строку бэка ("1", "2", "3") во фронтенд-Enum (+1)
        SystemRole = MapBackendRoleToFrontend(model.Role?.ToString());
    }


    private static UserRole MapBackendRoleToFrontend(string? rawRole)
    {
        if (string.IsNullOrEmpty(rawRole)) return UserRole.Bootstrap;

        // Чистый парсинг числовых индексов из PostgreSQL базы данных
        if (int.TryParse(rawRole.Trim(), out int backendId))
        {
            return (UserRole)(backendId + 1);
        }

        return UserRole.Bootstrap;
    }

    [RelayCommand]
    public void DeleteEmployee() // Сделано public для корректной генерации DeleteEmployeeCommand
    {
        // Логируем сам факт клика по кнопке из UI
        Temp.TempLogger.Log(
            $"[КЛИК УВОЛИТЬ] Сработала команда для Email: {Model.Email}. Проверяем наличие подписчиков на событие DeleteRequested: {DeleteRequested != null}");

        
        DeleteRequested?.Invoke(this, this);
    }

}