namespace Wms.Models.Shared;

public class User
{
    public string? Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public IList<string> Roles { get; set; } = new List<string>();
}
