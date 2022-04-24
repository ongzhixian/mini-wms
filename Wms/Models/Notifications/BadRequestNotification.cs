namespace Wms.Models.Notifications;

public class BadRequestNotification : NotificationBase
{
    public BadRequestNotification()
    {
        this.NotificationType = NotificationType.Error;
    }

    public BadRequestNotification(string message) : this()
    {
        this.Message = message;
    }
}
