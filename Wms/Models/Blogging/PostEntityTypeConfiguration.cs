using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Models.Data;

namespace Wms.Models.Data.Blogging;

public class PostEntityTypeConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        //builder
        //    .Property(b => b.Url)
        //    .IsRequired();
    }
}
