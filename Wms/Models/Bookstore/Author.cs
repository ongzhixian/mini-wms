using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;

namespace Wms.Models.Bookstore
{

    public class Author
    {
        // How to handle the case where we have repeated LastName and FirstName John Wayne

        public string? Id { get; set; }

        public string? LastName { get; set; }

        public string? FirstName { get; set; }

        public bool? Contract { get; set; }

        public ICollection<Address> Addresses { get; set; }
            = new Collection<Address>();

        public ICollection<Contact> Contacts { get; set; }
            = new Collection<Contact>();
    }

}
