using System;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Views.Manager.Parts.Templates;

namespace FenceFactory.Desktop.ViewModels.States.Manager.Visual.Materials.Sheet;

public class SheetPanelViewPreparer
{
    public void Prepare(SheetMaterialPanel view)
    {
        // Прямой доступ к свойству без FindControl
        // Вместо view.ProfileSelector.ItemsSource = Enum.GetValues<ProfileType>();
        view.ProfileSelector.ItemsSource = System.Enum.GetNames<ProfileType>();

       // view.ProfileSelector.SelectedIndex = 0;
    }
}