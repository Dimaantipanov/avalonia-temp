using FenceFactory.Desktop.Enums;

namespace FenceFactory.Desktop.ViewModels.States.Supplier;

/// <summary>
/// Скрипт состояния: Панель снабженца нехватка материалов.
/// Экран утреннего мониторинга дефицита для оплаченных заказов.
/// </summary>
public partial class SupplierDeficitViewModel : StateViewModelBase
{
    public override ApplicationState State => ApplicationState.SupplierDeficitView;

    public override void Activate()
    {
        // 1. Запрашиваем у фабрики составной бутерброд Нехватки материалов
        var view = FenceFactory.Desktop.Views.ViewFactory.CreateView(this.State);

        // 2. Назначаем себя хозяином логики
        view.DataContext = this;

        // 3. Инжектим готовый каркас в окно
        FenceFactory.Desktop.Views.Auth.AppShellWindow.Instance.SetContent(view);

        // [ЛОГ]
        Temp.TempLogger.Log($"[СКРИПТ СНАБЖЕНЦА]: Слайс {this.State} (Нехватка материалов) успешно выведен на холст!");
    }

    public override void Deactivate()
    {
        Temp.TempLogger.Log($"[СКРИПТ ПОДТВЕРЖДАЕТ]: Панель {this.State} успешно деактивирована и выгружена!");

    }
}