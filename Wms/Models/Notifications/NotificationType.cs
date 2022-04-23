namespace Wms.Models.Notifications;

[Flags]
public enum NotificationType
{
    Unknown     = 0x0,  // 0000
    Debug       = 0x1,  // 0001
    Information = 0x2,  // 0010
    Warning     = 0x4,  // 0100
    Error       = 0x8,  // 1000
}
