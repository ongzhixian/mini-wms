namespace Wms.Models.Shared;

public class User
{
    public string Username { get; set; }

    public string? Password { get; set; }

    public IList<string> Roles { get; set; } = new List<string>();

    public string PreferredApplication { get; set; }
}
