using System;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Views.Manager.Parts.Templates;

namespace FenceFactory.Desktop.ViewModels.States.Manager.Visual.Materials.Cement;

public class CementPanelViewPreparer
{
    public void Prepare(CementMaterialPanel view)
    {
        // Прямой доступ через твое каноничное свойство
        view.BrandSelector.ItemsSource = Enum.GetValues<CementBrand>();
        view.BrandSelector.SelectedIndex = 0;
    }
}