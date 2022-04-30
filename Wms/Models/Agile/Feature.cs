namespace Wms.Models.Data.Agile
{
    public class Feature
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<Story> Stories { get; set; } = new List<Story>();
        
    }
}
