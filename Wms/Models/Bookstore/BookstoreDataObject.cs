using System.Security.Cryptography;
using System.Text;

namespace Wms.Models.Bookstore;

public abstract class BookstoreDataObject
{
    private string _hash = string.Empty;

    public string Hash
    {
        get
        {
            return _hash;
        }
        protected set
        {
            _hash = value;
        }
    }

    protected abstract void updateHash();

    public string Base64MD5(string value)
    {
        return Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(value)));
    }
}