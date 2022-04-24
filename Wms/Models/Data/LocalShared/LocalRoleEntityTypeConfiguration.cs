using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Models.Data;

namespace Wms.Models.Data.LocalShared;

public class LocalRoleEntityTypeConfiguration : IEntityTypeConfiguration<LocalRole>
{
    public void Configure(EntityTypeBuilder<LocalRole> builder)
    {
        // Primary key

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name).IsRequired();

        // Relationship

        //builder.HasMany(c => c.)


    }
}
