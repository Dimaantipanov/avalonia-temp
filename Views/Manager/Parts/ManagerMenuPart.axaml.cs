using Avalonia.Controls;

namespace FenceFactory.Desktop.Views.Manager.Parts;

public partial class ManagerMenuPart : UserControl
{
    public ManagerMenuPart()
    {
        InitializeComponent();
    }

    // Кнопки переключения глобальных состояний менеджера
    public Button CreateTemplateButton => CreateTemplateNavButton;
    public Button FillDraftButton => FillDraftNavButton;
    public Button ActivateOrderButton => ActivateOrderNavButton;

    // Кнопка разлогина
    public Button ExitButton => ExitSystemButton;
    public void UpdateActiveButton(Enums.ApplicationState state)
    {
        // 1. Очищаем класс active со всех кнопок меню
        CreateTemplateNavButton.Classes.Remove("active");
        FillDraftNavButton.Classes.Remove("active");
        ActivateOrderNavButton.Classes.Remove("active");

        // 2. Вешаем только на кнопку текущего состояния
        switch (state)
        {
            case Enums.ApplicationState.ManagerMaterialTemplate:
                CreateTemplateNavButton.Classes.Add("active");
                break;
            case Enums.ApplicationState.ManagerDraft:
                FillDraftNavButton.Classes.Add("active");
                break;
            case Enums.ApplicationState.ManagerOrderActivation:
                ActivateOrderNavButton.Classes.Add("active");
                break;
        }
    }
}