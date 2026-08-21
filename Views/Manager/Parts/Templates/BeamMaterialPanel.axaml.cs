using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FenceFactory.Desktop.Views.Manager.Parts.Templates;

public partial class BeamMaterialPanel : UserControl
{
    public BeamMaterialPanel()
    {
        InitializeComponent();
    }

    // Выставляем наружу элементы числового ввода (NumericUpDown)
    public NumericUpDown WidthInput => BeamWidthInput;
    public NumericUpDown DepthInput => BeamDepthInput;
    public NumericUpDown ThicknessInput => BeamThicknessInput;
    public NumericUpDown LengthInput => BeamLengthInput;

    // Ссылка на текстовый блок превью SKU
    public TextBlock SkuPreview => BeamSkuPreviewText;
}