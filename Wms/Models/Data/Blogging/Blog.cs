namespace Wms.Models.Data.Blogging;

public class Blog
{
    public int BlogId { get; set; } = 0;

    public string Url { get; set; } = string.Empty;

    public List<Post> Posts { get; } = new();
}
