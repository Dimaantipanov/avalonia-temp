using FenceFactory.Desktop.Enums;

namespace FenceFactory.Desktop.ViewModels.States.Master;

/// <summary>
/// Скрипт状態: Мастер получение списка укомплектованных ордеров и отправка бригад на объекты.
/// Экран списания материалов со склада и распределения задач.
/// </summary>
public partial class MasterOrderReleaseViewModel : StateViewModelBase
{
    public override ApplicationState State => ApplicationState.MasterOrderRelease;

    public override void Activate()
    {
        // 1. Запрашиваем у фабрики составной бутерброд Выдачи ордеров
        var view = FenceFactory.Desktop.Views.ViewFactory.CreateView(this.State);

        // 2. Назначаем себя хозяином логики
        view.DataContext = this;

        // 3. Инжектим готовый каркас в окно
        FenceFactory.Desktop.Views.Auth.AppShellWindow.Instance.SetContent(view);

        // [ЛОГ]
        Temp.TempLogger.Log($"[СКРИПТ МАСТЕРА]: Слайс {this.State} (Выдача ордеров) успешно выведен на холст!");
    }

    public override void Deactivate()
    {
        Temp.TempLogger.Log($"[СКРИПТ ПОДТВЕРЖДАЕТ]: Панель {this.State} успешно деактивирована и выгружена!");

    }
}