namespace Wms.Models.Data.Agile
{
    public class Product
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public ICollection<Feature> Features { get; set; }
    }
}
