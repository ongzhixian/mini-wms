using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Wms.Models.Data;

namespace Wms.DbContexts;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }

    public string DbPath { get; }

    public BloggingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "blogging.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure your model.
        // This is the most powerful method of configuration
        // and allows configuration to be specified without modifying your entity classes.
        // Fluent API configuration has the highest precedence
        // and will override conventions and data annotations.

        //modelBuilder.Entity<Blog>()
        //    .Property(b => b.Url)
        //    .IsRequired();

        //new BlogEntityTypeConfiguration().Configure(modelBuilder.Entity<Blog>());

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(BlogEntityTypeConfiguration).Assembly);
    }
}
