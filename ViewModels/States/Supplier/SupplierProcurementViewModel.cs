using FenceFactory.Desktop.Enums;

namespace FenceFactory.Desktop.ViewModels.States.Supplier;

/// <summary>
/// Скрипт состояния: Оформление прихода от снабженца.
/// Экран оприходования ТМЦ, автоматического закрытия брони и распределения остатков.
/// </summary>
public partial class SupplierProcurementViewModel : StateViewModelBase
{
    public override ApplicationState State => ApplicationState.SupplierProcurement;

    public override void Activate()
    {
        // 1. Запрашиваем у фабрики составной бутерброд Оформления прихода
        var view = FenceFactory.Desktop.Views.ViewFactory.CreateView(this.State);

        // 2. Назначаем себя хозяином логики
        view.DataContext = this;

        // 3. Инжектим готовый каркас в окно
        FenceFactory.Desktop.Views.Auth.AppShellWindow.Instance.SetContent(view);

        // [ЛОГ]
        Temp.TempLogger.Log($"[СКРИПТ СНАБЖЕНЦА]: Слайс {this.State} (Оформление прихода) успешно выведен на холст!");
    }

    public override void Deactivate()
    {
        Temp.TempLogger.Log($"[СКРИПТ ПОДТВЕРЖДАЕТ]: Панель {this.State} успешно деактивирована и выгружена!");

    }
}