using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FenceFactory.Desktop.Views.Manager.Parts.Templates;

public partial class PillarMaterialPanel : UserControl
{
    public PillarMaterialPanel()
    {
        InitializeComponent();
    }

    // Выставляем наружу элементы числового ввода (NumericUpDown)
    public NumericUpDown WidthInput => PillarWidthInput;
    public NumericUpDown DepthInput => PillarDepthInput;
    public NumericUpDown ThicknessInput => PillarThicknessInput;
    public NumericUpDown HeightInput => PillarHeightInput;

    // Ссылка на текстовый блок превью SKU
    public TextBlock SkuPreview => PillarSkuPreviewText;
}
