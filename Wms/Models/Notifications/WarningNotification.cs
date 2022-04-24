namespace Wms.Models.Notifications;

public class WarningNotification : NotificationBase
{
    public WarningNotification()
    {
        this.NotificationType = NotificationType.Warning;
    }

    public WarningNotification(string message) : this()
    {
        this.Message = message;
    }
}
