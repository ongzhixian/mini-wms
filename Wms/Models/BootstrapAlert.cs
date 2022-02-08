namespace Wms.Models;

public class BootstrapAlert
{
    

    public string AlertType { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public static class AlertTypeName
    {
        public const string Danger = "danger";
        public const string Success = "success";
    }

    public BootstrapAlert() { }

    public BootstrapAlert(string description, string alertType = AlertTypeName.Danger) 
    { 
        this.AlertType = alertType;
        this.Description = description;
    }
}
