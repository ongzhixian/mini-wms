using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Wms.Models.Data.Agile;

public class MemberEntityTypeConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        // builder.HasKey(c => new {
        //     c.BlogId
        // });

        // builder
        //     .Property(b => b.Url)
        //     .IsRequired();
    }
}
