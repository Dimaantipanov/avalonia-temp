namespace FenceFactory.Desktop.Services.Network;

/// <summary>
/// Строгие клиентские роли для маршрутизации экранов в Avalonia.
/// </summary>
public enum UserRole
{
    Bootstrap = 0,
    Admin = 1,
    Manager = 2,
    Supplier = 3,
    Master = 4
}