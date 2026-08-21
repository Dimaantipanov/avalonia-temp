using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FenceFactory.Desktop.Enums;
using MaterialType = FenceFactory.Desktop.Enums.MaterialTabType;

namespace FenceFactory.Desktop.Views.Manager.Parts.Templates;

public partial class TopMenuContent : UserControl
{
    public TopMenuContent()
    {
        InitializeComponent();
    }

    // Выставляем наружу кнопки вкладок для коммутатора
    public Button TabSheet => TabSheetButton;
    public Button TabPillar => TabPillarButton;
    public Button TabBeam => TabBeamButton;
    public Button TabFastener => TabFastenerButton;
    public Button TabCement => TabCementButton;

    public Button TabInert => TabInertButton;

   
    // Метод переключения активной кнопки согласно энуму
    public void UpdateActiveTab(MaterialTabType activeTab)
    {
        ClearActiveStatus();

        Button targetButton = activeTab switch
        {
            MaterialTabType.Sheet => TabSheetButton,
            MaterialTabType.Pillar => TabPillarButton,
            MaterialTabType.Beam => TabBeamButton,
            MaterialTabType.Fastener => TabFastenerButton,
            MaterialTabType.Cement => TabCementButton,
            MaterialTabType.Inert => TabInertButton,
            _ => null!
        };

        targetButton.Classes.Add("active");
    }


    // Метод сброса класса active у всех кнопок
    private void ClearActiveStatus()
    {
        TabSheetButton.Classes.Remove("active");
        TabPillarButton.Classes.Remove("active");
        TabBeamButton.Classes.Remove("active");
        TabFastenerButton.Classes.Remove("active");
        TabCementButton.Classes.Remove("active");
        TabInertButton.Classes.Remove("active");
    }

    // Метод включения/отключения кликабельности всех кнопок
    public void SetButtonsEnabled(bool isEnabled)
    {
        TabSheetButton.IsEnabled = isEnabled;
        TabPillarButton.IsEnabled = isEnabled;
        TabBeamButton.IsEnabled = isEnabled;
        TabFastenerButton.IsEnabled = isEnabled;
        TabCementButton.IsEnabled = isEnabled;
        TabInertButton.IsEnabled = isEnabled;
    }
}