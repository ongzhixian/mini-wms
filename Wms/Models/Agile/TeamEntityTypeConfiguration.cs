using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wms.Models.Data.Agile;

public class TeamEntityTypeConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        // builder.HasKey(c => new {
        //     c.BlogId
        // });

        // builder
        //     .Property(b => b.Url)
        //     .IsRequired();
    }
}
