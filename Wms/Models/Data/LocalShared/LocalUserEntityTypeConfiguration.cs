using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Wms.Models.Data;

namespace Wms.Models.Data.LocalShared;

public class LocalUserEntityTypeConfiguration : IEntityTypeConfiguration<LocalUser>
{
    public void Configure(EntityTypeBuilder<LocalUser> builder)
    {
        // Primary key
        builder.HasKey(m => m.Id);

        builder.Property(b => b.Username).IsRequired();

        builder.Property(b => b.Password).IsRequired();

        // Update timestamps

        var utcNow = DateTime.UtcNow;
        builder.Property(m => m.CreationTime).HasDefaultValue(utcNow);
        builder.Property(m => m.LastUpdateTime).HasDefaultValue(utcNow);
        builder.Property(m => m.PasswordLastUpdateTime).HasDefaultValue(utcNow);

        // EF Navigation Props

        builder
            .HasMany(m => m.Roles)
            .WithMany(m => m.LocalUsers)
            .UsingEntity(j => j.ToTable("LocalRole_LocalUser"));
    }
}
