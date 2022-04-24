namespace Wms.Models.Data;

// Base class for all Entity
public abstract class EntityBase
{
    public DateTime CreationTime { get; set; }

    public DateTime LastUpdateTime { get; set; }
}
