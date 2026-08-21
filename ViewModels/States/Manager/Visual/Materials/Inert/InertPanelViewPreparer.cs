using System;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Views.Manager.Parts.Templates;

namespace FenceFactory.Desktop.ViewModels.States.Manager.Visual.Materials.Inert;

public class InertPanelViewPreparer
{
    public void Prepare(InertMaterialPanel view)
    {
        // Прямой доступ без FindControl через твое кастомное свойство
        view.TypeSelector.ItemsSource = Enum.GetValues<InertType>();
        view.TypeSelector.SelectedIndex = 0;
    }
}