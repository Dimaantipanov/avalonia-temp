using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FenceFactory.Desktop.Views.Manager.Parts.Orders;

public partial class OrderPanel : UserControl
{
    public OrderPanel()
    {
        InitializeComponent();
    }

    // Таблица заказов
    public DataGrid OrdersTable => OrdersDataGrid;

    // Текст ошибки
    public TextBlock ErrorText => ErrorBlockText;
}