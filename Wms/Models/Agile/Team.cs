namespace Wms.Models.Data.Agile
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<Member> Members { get; set; } = new List<Member>();
    }
}
