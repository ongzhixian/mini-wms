namespace Wms.Models.Bookstore;

public class Contact : BookstoreDataObject
{
    private string _contactType;

    public string ContactType
    {
        get 
        { 
            return _contactType; 
        }
        set 
        { 
            _contactType = value;
            updateHash();
        }
    }

    private string _value;

    public string Value
    {
        get 
        { 
            return _value; 
        }
        set 
        { 
            _value = value;
            updateHash();
        }
    }

    public override string ToString()
    {
        return $"Contact type: {ContactType}, Value: {Value}";
    }

    protected override void updateHash()
    {
        Hash = Base64MD5($"{_contactType}{_value}");
    }
}
