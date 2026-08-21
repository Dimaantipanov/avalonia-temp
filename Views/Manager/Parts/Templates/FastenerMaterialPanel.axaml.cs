using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FenceFactory.Desktop.Views.Manager.Parts.Templates;

public partial class FastenerMaterialPanel : UserControl
{
    public FastenerMaterialPanel()
    {
        InitializeComponent();
    }

    // Выставляем наружу элементы числового ввода (NumericUpDown)
    public NumericUpDown LengthInput => FastenerLengthInput;

    // Ссылка на текстовый блок превью SKU
    public TextBlock SkuPreview => FastenerSkuPreviewText;
}