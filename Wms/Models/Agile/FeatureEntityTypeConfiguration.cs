using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wms.Models.Data.Agile;

public class FeatureEntityTypeConfiguration : IEntityTypeConfiguration<Feature>
{
    public void Configure(EntityTypeBuilder<Feature> builder)
    {
        // builder.HasKey(c => new {
        //     c.BlogId
        // });

        // builder
        //     .Property(b => b.Url)
        //     .IsRequired();
    }
}
