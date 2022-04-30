namespace Wms.Models.Data.Agile
{
    public class Story
    {
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int StoryPoints { get; set; } = 0;

        // public IList<string> AcceptanceCriteria { get; set; }

        // public IList<string> TechnicalAcceptanceCriteria { get; set; }

    }
}
