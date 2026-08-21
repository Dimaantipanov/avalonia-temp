using FenceFactory.Desktop.Enums;

namespace FenceFactory.Desktop.ViewModels.States.Master;

/// <summary>
/// Скрипт состояния: Мастер удаление бригад.
/// Экран расформирования монтажных звеньев и зачистки истории выездов.
/// </summary>
public partial class MasterTeamDeletionViewModel : StateViewModelBase
{
    public override ApplicationState State => ApplicationState.MasterTeamDeletion;

    public override void Activate()
    {
        // 1. Запрашиваем у фабрики составной бутерброд Удаления бригад
        var view = FenceFactory.Desktop.Views.ViewFactory.CreateView(this.State);

        // 2. Назначаем себя хозяином логики
        view.DataContext = this;

        // 3. Инжектим готовый каркас в окно
        FenceFactory.Desktop.Views.Auth.AppShellWindow.Instance.SetContent(view);

        // [ЛОГ]
        Temp.TempLogger.Log($"[СКРИПТ МАСТЕРА]: Слайс {this.State} (Удаление бригад) успешно выведен на холст!");
    }

    public override void Deactivate()
    {
        Temp.TempLogger.Log($"[СКРИПТ ПОДТВЕРЖДАЕТ]: Панель {this.State} успешно деактивирована и выгружена!");

    }
}
