using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;

namespace Wms.Models.Bookstore
{

    public class Author
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        private string? _hash;
        public string? Hash { 
            get
            {
                if (string.IsNullOrWhiteSpace(_hash))
                {
                    using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        sb.Append(FirstName);
                        sb.Append(LastName);
                        sb.Append(Contract);
                        sb.Append(Addresses);
                        sb.Append(Contacts);
                        _hash = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString())));
                    }
                        
                }
                return _hash;
            }
            set
            {
                _hash = value;
            }
        }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        //public string? Phone { get; set; }

        //public string? Address { get; set; }

        //public string? City { get; set; }
        
        //public string? State { get; set; }
        
        //public string? Zip { get; set; }

        public bool? Contract { get; set; }

        public ICollection<Address> Addresses { get; set; }
        = new Collection<Address>();

        public ICollection<Contact> Contacts { get; set; }
            = new Collection<Contact>();
    }

}
