namespace Wms.Models.Notifications;

public abstract class NotificationBase
{
    public NotificationType NotificationType { get; set; }

    public string Message { get; set; } = string.Empty;
}