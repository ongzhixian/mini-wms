using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wms.Models.Data.Agile;

public class StoryEntityTypeConfiguration : IEntityTypeConfiguration<Story>
{
    public void Configure(EntityTypeBuilder<Story> builder)
    {
        // builder.HasKey(c => new {
        //     c.BlogId
        // });

        // builder
        //     .Property(b => b.Url)
        //     .IsRequired();
    }
}
