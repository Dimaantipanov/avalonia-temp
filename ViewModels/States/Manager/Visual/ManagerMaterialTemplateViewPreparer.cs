using Avalonia.Controls;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Views.Manager.Parts.Templates;


namespace FenceFactory.Desktop.ViewModels.States.Manager.Visual;

public class ManagerMaterialTemplateViewPreparer
{
    public UserControl CreateAndPrepare(ManagerMaterialTemplateViewModel viewModel)
    {
        var view = Views.ViewFactory.CreateView(viewModel.State) as MaterialTemplatePanel;
        if (view == null) return new UserControl();

        view.DataContext = viewModel;

        return view;
    }

    // Метод динамической подмены мини-панелей параметров в центре экрана
    public void SwitchSubPanel(ContentControl container, MaterialTabType tabType, ManagerMaterialTemplateViewModel viewModel)
    {
        if (container == null) return;

        // Создаем конкретный UserControl в зависимости от выбранной вкладки меню
        UserControl? subView = tabType switch
        {
            MaterialTabType.Sheet => new SheetMaterialPanel(),
            MaterialTabType.Pillar => new PillarMaterialPanel(),
            MaterialTabType.Beam => new BeamMaterialPanel(),
            MaterialTabType.Fastener => new FastenerMaterialPanel(),
            MaterialTabType.Cement => new CementMaterialPanel(),
            MaterialTabType.Inert => new InertMaterialPanel(),
            _ => null
        };

        if (subView == null) return;

        // Накатываем DataContext текущей ViewModel
        subView.DataContext = viewModel;

        // Физически подменяем старую разметку на новую
        container.Content = subView;
    }
}