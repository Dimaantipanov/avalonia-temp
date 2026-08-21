using System;
using Avalonia.Controls;
using FenceFactory.Desktop.Enums; // Библиотека, где лежат твои энумы

namespace FenceFactory.Desktop.Views.Manager.Parts.Templates;

public partial class InertMaterialPanel : UserControl
{
    public InertMaterialPanel()
    {
        InitializeComponent();

        // Сразу прибиваем локальный энум инертных материалов в конструкторе
        InertTypeComboBox.ItemsSource = Enum.GetNames<InertType>();
        InertTypeComboBox.SelectedIndex = 0;
    }

    // Твои каноничные свойства для коммутатора
    public ComboBox TypeSelector => InertTypeComboBox;
    public NumericUpDown WeightInput => InertWeightInput;
    public TextBlock SkuPreview => InertSkuPreviewText;
}