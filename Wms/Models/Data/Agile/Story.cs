namespace Wms.Models.Data.Agile
{
    public class Story
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public int StoryPoints { get; set; }

        public IList<string> AcceptanceCriteria { get; set; }

        public IList<string> TechnicalAcceptanceCriteria { get; set; }

    }
}
