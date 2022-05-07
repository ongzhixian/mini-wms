namespace Wms.Models.Bookstore;

public class Contact : BookstoreDataObject
{
    public string ContactType { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    //public string HashCode {
    //    get { return Base64MD5($"{ContactType}{Value}"); }
    //}

    public override string ToString()
    {
        return $"Contact type: {ContactType}, Value: {Value}";
    }

}
