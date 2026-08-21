using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using FenceFactory.Desktop.Enums;
using System;
using FenceFactory.Desktop.Temp;

namespace FenceFactory.Desktop.Views.Manager.Parts.Templates;

public partial class SheetMaterialPanel : UserControl
{
    public SheetMaterialPanel()
    {
        InitializeComponent();
        
        // Напрямую скармливаем энумы контролу при его создании
        ProfileComboBox.ItemsSource = Enum.GetNames<ProfileType>();
        ProfileComboBox.SelectedIndex = 0;

        // КАНОН: Лог перенесен внутрь конструктора, теперь он скомпилируется!
        TempLogger.Log("[ФИЗИЧЕСКИЙ UI] Панель SheetMaterialPanel РОДИЛАСЬ в памяти!");
    }

    // Выставляем наружу элементы управления для профлиста
    public ComboBox ProfileSelector => ProfileComboBox;
    public NumericUpDown ThicknessInput => SheetThicknessInput;
    public NumericUpDown HeightInput => SheetHeightInput;

    // Ссылка на текстовый блок превью SKU
    public TextBlock SkuPreview => SkuPreviewText;
}