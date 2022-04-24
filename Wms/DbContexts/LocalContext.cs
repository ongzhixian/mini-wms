using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Wms.Models.Data;
using Wms.Models.Data.LocalShared;

namespace Wms.DbContexts;

public class LocalContext : DbContext
{
    public DbSet<LocalUser> LocalUsers => Set<LocalUser>();

    public DbSet<LocalRole> LocalRoles => Set<LocalRole>();

    public LocalContext(DbContextOptions<LocalContext> options) : base(options)
    {
    }

    public LocalContext()
    {
        // Uncomment if we want to override the options passed to provider
        //var folder = Environment.SpecialFolder.LocalApplicationData;
        //var path = Environment.GetFolderPath(folder);
        //DbPath = System.IO.Path.Join(path, "blogging.db");
        //Console.WriteLine(DbPath);
    }

    // Uncomment if we want to override the options passed to provider
    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //    => options.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure your model.
        // This is the most powerful method of configuration
        // and allows configuration to be specified without modifying your entity classes.
        // Fluent API configuration has the highest precedence
        // and will override conventions and data annotations.

        // Method 1. Model Url property of Blog class
        //modelBuilder.Entity<Blog>()
        //    .Property(b => b.Url)
        //    .IsRequired();

        // Method 2. Model Blog class in BlogEntityTypeConfiguration
        //new BlogEntityTypeConfiguration().Configure(modelBuilder.Entity<Blog>());

        // Method 3. Model from EntityTypeConfiguration classes in assembly
        // This can be better written.
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(LocalUserEntityTypeConfiguration).Assembly);

        // Relation

        //modelBuilder.Entity<LocalUser>()
        //    .HasMany(t => t.Roles)
        //    .WithMany(t => t.LocalUsers)
        //    .UsingEntity(j => j.ToTable("LocalRole_LocalUser")); ;

    }
}
