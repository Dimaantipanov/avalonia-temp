using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FenceFactory.Desktop.Views.Manager.Parts.Templates;

public partial class MaterialTemplatePanel : UserControl
{
    public MaterialTemplatePanel()
    {
        InitializeComponent();
    }

    // Выставляем наружу прямые ссылки для Триады
    public Button SaveButton => SaveTemplateButton;
    public TextBlock ErrorText => ErrorBlockText;
    public ContentControl PanelContainer => MaterialPanelContainer;
    public TopMenuContent TopMenuControl => TopMenu;
}