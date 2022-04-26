using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Wms.Models.Data;
using Wms.Models.Data.Agile;

namespace Wms.DbContexts;

public class AgileContext : DbContext
{
    public DbSet<Feature> Features => Set<Feature>();

    public DbSet<Member> Members => Set<Member>();

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Story> Stories => Set<Story>();

    public DbSet<Team> Teams => Set<Team>();

    public AgileContext(DbContextOptions<AgileContext> options) : base(options)
    {
    }

    public AgileContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<Course>().ToTable("Course");

        // modelBuilder.Entity<Enrollment>().ToTable("Enrollment");

        // modelBuilder.Entity<Student>().ToTable("Student");
    }
}
