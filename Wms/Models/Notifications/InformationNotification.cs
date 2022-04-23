namespace Wms.Models.Notifications;

public class InformationNotification : NotificationBase
{
    public InformationNotification()
    {
        this.NotificationType = NotificationType.Information;
    }

    public InformationNotification(string message) : this()
    {
        this.Message = message;
    }
}
