using System;
using Avalonia.Controls;
using FenceFactory.Desktop.Enums; // Библиотека, где лежат твои энумы

namespace FenceFactory.Desktop.Views.Manager.Parts.Templates;

public partial class CementMaterialPanel : UserControl
{
    public CementMaterialPanel()
    {
        InitializeComponent();

        // Взламываем систему: нативно скармливаем текстовые имена энумов цемента при рождении
        CementBrandComboBox.ItemsSource = Enum.GetNames<CementBrand>();
        CementBrandComboBox.SelectedIndex = 0;
    }

    // Твои каноничные свойства для коммутатора цемента
    public ComboBox BrandSelector => CementBrandComboBox;
    public NumericUpDown WeightInput => CementWeightInput;
    public TextBlock SkuPreview => CementSkuPreviewText;
}