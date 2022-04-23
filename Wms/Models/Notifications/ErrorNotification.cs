namespace Wms.Models.Notifications;

public class ErrorNotification : NotificationBase
{
    public ErrorNotification()
    {
        this.NotificationType = NotificationType.Error;
    }

    public ErrorNotification(string message) : this()
    {
        this.Message = message;
    }
}
