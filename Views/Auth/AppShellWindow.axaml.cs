using Avalonia.Controls;

namespace FenceFactory.Desktop.Views.Auth;

public partial class AppShellWindow : Window
{
    // Статический синглтон для прямого управления из Скриптов Состояний
    public static AppShellWindow Instance { get; private set; } = null!;

    // Долгоживущее хранилище сессии и токенов для всех экранов системы
    public Services.Network.NetworkSession CurrentSession { get; } = new();

    public AppShellWindow()
    {
        InitializeComponent();
        Instance = this; // Регистрируем холст в памяти в момент запуска
        
    }

    /// <summary>
    /// Прямой инжект панели внутрь окна-рамки. Старая панель уничтожается.
    /// </summary>
    public void SetContent(Control panel)
    {
        this.Content = panel;
    }
}