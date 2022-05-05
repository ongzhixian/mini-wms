namespace Wms.Models.Bookstore;

public class Address
{
    public string? Street { get; set; }

    public string? Unit { get; set; }

    public string? City { get; set; }

    // We use reegion to represent state/province
    public string? Region { get; set; }

    // Postal code
    public string? PostCode { get; set; }

    public string? Country { get; set; }
}
