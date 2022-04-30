﻿namespace Wms.Models.Data.Blogging;

public class Post
{
    public int PostId { get; set; }

    public string Title { get; set; } = string.Empty;
    
    public string Content { get; set; } = string.Empty;

    public int BlogId { get; set; }

    public Blog Blog { get; set; } = new Blog();
}