using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wms.Models.Data.Blogging;

//public class BloggingContextFactory : IDesignTimeDbContextFactory<BloggingContext>
//{
//    public BloggingContext CreateDbContext(string[] args)
//    {
//        var optionsBuilder = new DbContextOptionsBuilder<BloggingContext>();

//        optionsBuilder.UseSqlite("Data Source=blog.db");

//        return new BloggingContext(optionsBuilder.Options);
//    }
//}

public class BlogEntityTypeConfiguration : IEntityTypeConfiguration<Blog>
{
    public void Configure(EntityTypeBuilder<Blog> builder)
    {
        builder.HasKey(c => new {
            c.BlogId
        });

        builder
            .Property(b => b.Url)
            .IsRequired();
    }
}
