namespace Wms.Models.Bookstore;

public class Address : BookstoreDataObject
{
    public string? Street { get; set; }

    public string? Unit { get; set; }

    public string? City { get; set; }

    // We use reegion to represent state/province
    public string? Region { get; set; }

    // Postal code
    public string? PostCode { get; set; }

    public string? Country { get; set; }

    //public override string Hash() => Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes($"{ContactType}{Value}")));

    public string ToStringX()
    {
        return $"Street: {Street}, Unit: {Unit}, City: {City}, Region: {Region}, Post code: {PostCode}, Country: {Country}";
    }

    protected override void updateHash()
    {
        throw new NotImplementedException();
    }
}
