namespace Wms.Models.Data.LocalShared;

public class LocalRole : EntityBase
{
    public int Id { get; set; }

    public string Name { get; set; } = String.Empty;


    // EF Navigation

    public ICollection<LocalUser> LocalUsers { get; set; } = new List<LocalUser>();
}
