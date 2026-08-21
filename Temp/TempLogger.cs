using System;
using System.Collections.ObjectModel;
using Avalonia.Threading;

namespace FenceFactory.Desktop.Temp;

/// <summary>
/// Временный отладочный логгер для отслеживания состояний.
/// Чтобы полностью все отключить, достаточно поменять IsEnabled на false.
/// </summary>
public static class TempLogger
{
    // Главный тумблер: true - окно логов работает, false - полностью отключено
    public static readonly bool IsEnabled = true;

    // Коллекция строк, которую будет читать наше будущее окно логов
    public static ObservableCollection<string> LogLines { get; } = new();

    // Ссылка на само окно, чтобы мы могли им управлять (например, закрыть при выходе)
    private static Avalonia.Controls.Window? _logWindow;

    public static void Start()
    {
        if (!IsEnabled) return;

        // Создаем экземпляр нашего нового окна логов
        _logWindow = new TempLogWindow();
        // ХИРУРГИЯ: Окно логов больше не будет забирать фокус при кликах на него
        _logWindow.Focusable = false;
    
        // Отображаем его параллельно основному интерфейсу
        _logWindow.Show();
    }


    /// <summary>
    /// Метод для отправки логов из любой точки вашей стейт-машины
    /// </summary>
    public static void Log(string message)
    {
        if (!IsEnabled) return;

        // Безопасно добавляем запись в UI-поток Avalonia
        Dispatcher.UIThread.Post(() =>
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            LogLines.Add($"[{time}] {message}");
        });
    }

    public static void Stop()
    {
        if (_logWindow != null)
        {
            _logWindow.Close();
            _logWindow = null;
        }
        LogLines.Clear();
    }
}