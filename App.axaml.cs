using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using FenceFactory.Desktop.ViewModels;



namespace FenceFactory.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            DisableAvaloniaDataAnnotationValidation();

            // ХИРУРГИЯ: Запускаем монументальное окно-оболочку AppShellWindow
            desktop.MainWindow = new FenceFactory.Desktop.Views.Auth.AppShellWindow
            {
                DataContext = new MainWindowViewModel(),
            };

            if (desktop.MainWindow.DataContext is MainWindowViewModel vm)
            {
                Temp.TempKeyboardTracker.Start(vm);
    
                // Запуск нашего нового окна логов
                Temp.TempLogger.Start();
            }
        }


        base.OnFrameworkInitializationCompleted();
    }


    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}