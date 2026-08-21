using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Temp;
using FenceFactory.Desktop.Views.Manager.Parts.Templates;
using FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Materials.Beam;
using FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Materials.Cement;
using FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Materials.Fastener;
using FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Materials.Inert;
using FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Materials.Pillar;
using FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Materials.Sheet;

namespace FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Materials.Dispatchers;

public class MaterialSubCommutatorDispatcher
{
    private object? _activeCommutator;

    public void SwitchMaterial(MaterialTabType type)
    {
        // КАНОН: Логируем только сам факт вызова стейта
        TempLogger.Log($"[Dispatcher] SwitchMaterial invoked with type: {type}");

        // Выключаем старый активный коммутатор по канону для предотвращения утечек
        DisconnectActive();

        // Запускаем нужную логику. 
        // Дочерние коммутаторы сами внутри себя затормозят поток через Post и поймают свои панели без NULL!
        switch (type)
        {
            case MaterialTabType.Sheet:
                var sheet = new SheetPanelCommutator();
                _activeCommutator = sheet;
                sheet.Connect();
                break;
                
            case MaterialTabType.Cement:
                // Сюда потом пропишем цемент по такой же схеме с тормозом
                break;
                
            case MaterialTabType.Inert:
                // Сюда встанет инертный коммутатор
                break;
        }
    }

    public void DisconnectActive()
    {
        if (_activeCommutator == null) return;
    
        ((dynamic)_activeCommutator).Disconnect();
        _activeCommutator = null;
    }
}