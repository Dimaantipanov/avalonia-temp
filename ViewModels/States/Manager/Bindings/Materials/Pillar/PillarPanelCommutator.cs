using Avalonia.Controls;
using FenceFactory.Desktop.Views.Auth;
using FenceFactory.Desktop.Views.Manager.Parts.Templates;
using FenceFactory.Desktop.ViewModels.States.Manager.Visual.Materials.Pillar;

namespace FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Materials.Pillar;

public class PillarPanelCommutator
{
    private PillarMaterialPanel? _view;

    public void Connect()
    {
        // Достаем активный контент из контейнера главного экрана через раму
        var mainPanel = AppShellWindow.Instance.Content as MaterialTemplatePanel;
        _view = mainPanel?.PanelContainer.Content as PillarMaterialPanel;

        if (_view == null) return;

        // Локальная инициализация препарера и прямая настройка визуала
        var preparer = new PillarPanelViewPreparer();
        preparer.Prepare(_view);

        // Дальше чистые и быстрые подписки без FindControl: _view.MyControl.Event += ...
    }

    public void Disconnect()
    {
        if (_view != null)
        {
            // Здесь будут отписки от событий: _view.MyControl.Event -= ...
        }

        // Строго зануляем ссылку по канону для предотвращения утечек памяти
        _view = null;
    }
}