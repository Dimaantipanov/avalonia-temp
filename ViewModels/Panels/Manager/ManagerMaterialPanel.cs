namespace FenceFactory.Client.Manager;
using FenceFactory.Desktop.Enums;

public class ManagerMaterialPanel
{
    // Отслеживание выбранной вкладки материала на экране
    public string SelectedMaterialType { get; set; } = "Sheet"; 

    // --- Листовые (Профлист) ---
    public ProfileType Profile { get; set; }
    public string ThicknessRaw { get; set; } = string.Empty;
    public string HeightRaw { get; set; } = string.Empty;

    // --- Столбы ---
    public string PillarWidthRaw { get; set; } = string.Empty;
    public string PillarDepthRaw { get; set; } = string.Empty;
    public string PillarThicknessRaw { get; set; } = string.Empty;
    public string PillarHeightRaw { get; set; } = string.Empty;

    // --- Прожилины (Лаги) ---
    public string BeamWidthRaw { get; set; } = string.Empty;
    public string BeamDepthRaw { get; set; } = string.Empty;
    public string BeamThicknessRaw { get; set; } = string.Empty;
    public string BeamLengthRaw { get; set; } = string.Empty;

    // --- Крепеж ---
    public string FastenerLengthRaw { get; set; } = string.Empty;

    // --- Цемент ---
    public CementBrand CementBrand { get; set; }
    public string CementWeightRaw { get; set; } = string.Empty;

    // --- Инертные ---
    public InertType InertType { get; set; }
    public string InertWeightRaw { get; set; } = string.Empty;
}
