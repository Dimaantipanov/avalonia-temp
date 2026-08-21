using Avalonia.Controls;

namespace FenceFactory.Desktop.Views.Manager.Parts.Drafts;

public partial class DraftPanel : UserControl
{
    public DraftPanel()
    {
        InitializeComponent();
    }

    // Поля ввода
    public TextBox AddressField => AddressTextBox;
    public ComboBox MaterialSelector => MaterialComboBox;
    public TextBox AmountField => AmountTextBox;

    // Таблица сметы
    public DataGrid EstimateTable => EstimateDataGrid;

    // Кнопки управления
    public Button AddButton => AddMaterialButton;
    public Button SubmitButton => SubmitOrderButton;

    // Статус ошибки
    public TextBlock ErrorText => ErrorBlockText;
}