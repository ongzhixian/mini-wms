using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wms.Models.Data.Agile;

public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // builder.HasKey(c => new {
        //     c.BlogId
        // });

        // builder
        //     .Property(b => b.Url)
        //     .IsRequired();
    }
}
