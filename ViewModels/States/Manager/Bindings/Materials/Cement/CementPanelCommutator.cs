using Avalonia.Controls;
using FenceFactory.Desktop.Views.Auth;
using FenceFactory.Desktop.Views.Manager.Parts.Templates;
using FenceFactory.Desktop.ViewModels.States.Manager.Visual.Materials.Cement;

namespace FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Materials.Cement;

public class CementPanelCommutator
{
    private CementMaterialPanel? _view;

    public void Connect()
    {
        // Достаем активный контент из контейнера главного экрана через раму
        var mainPanel = AppShellWindow.Instance.Content as MaterialTemplatePanel;
        _view = mainPanel?.PanelContainer.Content as CementMaterialPanel;

        if (_view == null) return;

        // Локальная инициализация препарера и прямая настройка визуала (Enum)
        var preparer = new CementPanelViewPreparer();
        preparer.Prepare(_view);

        // Дальше чистые и быстрые подписки без FindControl: _view.BrandSelector.SelectionChanged += ...
    }

    public void Disconnect()
    {
        if (_view != null)
        {
            // Здесь будут отписки от событий: _view.BrandSelector.SelectionChanged -= ...
        }

        // Строго зануляем ссылку по канону для предотвращения утечек памяти
        _view = null;
    }
}