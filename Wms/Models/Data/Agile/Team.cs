namespace Wms.Models.Data.Agile
{
    public class Team
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Member> Members { get; set; }
    }
}
