namespace Wms.Models.Data.LocalShared;

public class LocalUser : EntityBase
{
    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public DateTime PasswordLastUpdateTime { get; set; } = DateTime.UtcNow;

    public ICollection<LocalRole> Roles { get; set; } = new List<LocalRole>();
}
