using FenceFactory.Desktop.Enums;

namespace FenceFactory.Desktop.ViewModels.States.Master;

/// <summary>
/// Скрипт состояния: Мастер регистрация бригад.
/// Экран формирования и авторизации новых монтажных звеньев.
/// </summary>
public partial class MasterTeamRegistrationViewModel : StateViewModelBase
{
    public override ApplicationState State => ApplicationState.MasterTeamRegistration;

    public override void Activate()
    {
        // 1. Запрашиваем у фабрики составной бутерброд Регистрации бригад
        var view = FenceFactory.Desktop.Views.ViewFactory.CreateView(this.State);

        // 2. Назначаем себя хозяином логики
        view.DataContext = this;

        // 3. Инжектим готовый каркас Мастера на холст приложения
        FenceFactory.Desktop.Views.Auth.AppShellWindow.Instance.SetContent(view);

        // [ЛОГ]
        Temp.TempLogger.Log($"[СКРИПТ МАСТЕРА]: Слайс {this.State} (Регистрация бригад) успешно выведен на холст!");
    }

    public override void Deactivate()
    {
        Temp.TempLogger.Log($"[СКРИПТ ПОДТВЕРЖДАЕТ]: Панель {this.State} успешно деактивирована и выгружена!");

    }
}