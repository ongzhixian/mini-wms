namespace Wms.Models.Data.Agile
{
    public class Feature
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Story> Stories { get; set; }
        
    }
}
