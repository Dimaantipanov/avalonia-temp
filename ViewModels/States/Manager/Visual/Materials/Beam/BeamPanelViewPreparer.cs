using System;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Views.Manager.Parts.Templates;

namespace FenceFactory.Desktop.ViewModels.States.Manager.Visual.Materials.Beam;

public class BeamPanelViewPreparer
{
    public void Prepare(BeamMaterialPanel view)
    {
        // Настройка выпадающего списка типов прожилин напрямую без FindControl
        // view.BeamTypeSelector.ItemsSource = Enum.GetValues<BeamType>();
        // view.BeamTypeSelector.SelectedIndex = 0;
    }
}