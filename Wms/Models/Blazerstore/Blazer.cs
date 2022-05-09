namespace Wms.Models.Blazerstore;

public class Blazer
{
    public string? Id { get; set; }

    public string? Size { get; set; }

    public string? Status { get; set; }

    public DateTime? Borrow { get; set; }

    public DateTime? Due { get; set; }
}
