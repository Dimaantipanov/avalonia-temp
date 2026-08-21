using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using FenceFactory.Desktop.Views.Auth;
using FenceFactory.Desktop.Views.Manager.Parts.Templates;
using FenceFactory.Desktop.ViewModels.States.Manager.Visual.Materials.Sheet;
using FenceFactory.Desktop.Enums;
using FenceFactory.Desktop.Temp;

namespace FenceFactory.Desktop.ViewModels.States.Manager.Bindings.Materials.Sheet;

public class SheetPanelCommutator
{
    private SheetMaterialPanel? _view;
    private MaterialTemplatePanel? _mainPanel;

    public void Connect()
    {
        // 1. Сразу берём главную рамку
        _mainPanel = AppShellWindow.Instance.Content as MaterialTemplatePanel;
        if (_mainPanel == null) return;

        // 2. ГЕЙМДЕВ-ЦИКЛ: Если Авалония тупит и выдаёт NULL, мы принудительно заставляем её
        // обновить макет прямо в этой строчке кода, пока она не выдаст нам SheetMaterialPanel!
        int safetyCounter = 0;
        while (_view == null && safetyCounter < 100)
        {
            _view = _mainPanel.PanelContainer.Content as SheetMaterialPanel;
            
            if (_view == null)
            {
                // Жесткий пинок ленивому движку фреймворка в этой же строчке потока
                _mainPanel.PanelContainer.ApplyTemplate();
                _mainPanel.PanelContainer.InvalidateMeasure();
                _mainPanel.PanelContainer.InvalidateArrange();
            }
            
            safetyCounter++;
        }

        // Тестовый лог: теперь он ОБЯЗАН загореться, потому что код синхронный!
        TempLogger.Log($"[КОММУТАТОР ТЕСТ] Проверок сделано: {safetyCounter}. Панель: {_view?.GetType().Name ?? "NULL"}");

        if (_view == null) return;

        // 3. Если панель выбита из ленивого движка — настраиваем её
        var preparer = new SheetPanelViewPreparer();
        preparer.Prepare(_view);

        // 4. Подписываемся на вечную кнопку
        _mainPanel.SaveButton.Click += OnSaveTemplateClick;
        
        TempLogger.Log("[SheetCommutator] ЖЕЛЕЗОБЕТОННЫЙ УСПЕХ: Связь установлена!");
    }

    private async void OnSaveTemplateClick(object? sender, RoutedEventArgs e)
    {
        // Жесткая проверка: если ссылки не готовы, никуда не идем
        if (_view == null || _mainPanel == null) return;

        try
        {
            // Блокируем кнопку на время имитации сетевого запроса
            _mainPanel.SaveButton.IsEnabled = false;

            // Прямолинейно и безопасно считываем данные из живых полей в ОЗУ
            string profile = _view.ProfileSelector.SelectedItem?.ToString() ?? "C8";
            decimal thickness = _view.ThicknessInput.Value ?? 45;
            decimal height = _view.HeightInput.Value ?? 2000;

            TempLogger.Log($"[БИЗНЕС-ЛОГИКА] Данные Профлиста готовы к отправке: {profile}, {thickness}мм, {height}мм.");

            // Имитируем задержку сети бэкенда (Правило №5)
            await System.Threading.Tasks.Task.Delay(1000); 

            TempLogger.Log("[БИЗНЕС-ЛОГИКА] Шаблон Профлиста успешно сохранен на сервере!");
        }
        catch (Exception ex)
        {
            _mainPanel.ErrorText.Text = $"Ошибка сохранения: {ex.Message}";
        }
        finally
        {
            // В любом случае возвращаем доступность кнопке
            if (_mainPanel != null) _mainPanel.SaveButton.IsEnabled = true;
        }
    }

    public void Disconnect()
    {
        // Строго отписываемся от вечной кнопки рамки, чтобы не копить мусор в памяти
        if (_mainPanel != null)
        {
            _mainPanel.SaveButton.Click -= OnSaveTemplateClick;
        }

        // Зануляем ссылки по канону
        _view = null;
        _mainPanel = null;
        
        TempLogger.Log("[SheetCommutator] ДИСКОННЕКТ: Ссылки очищены.");
    }
}
