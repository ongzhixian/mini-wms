using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Models.Data;

namespace Wms.DbContexts;

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
        builder
            .Property(b => b.Url)
            .IsRequired();
    }
}
