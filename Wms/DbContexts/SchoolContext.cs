using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Wms.Models.Data;
using Wms.Models.Data.Education;
using Wms.Models.Data.LocalShared;

namespace Wms.DbContexts;

public class SchoolContext : DbContext
{
    public DbSet<Student> Students => Set<Student>();

    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    
    public DbSet<Course> Courses => Set<Course>();

    public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
    {
    }

    public SchoolContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Course>().ToTable("Course");

        modelBuilder.Entity<Enrollment>().ToTable("Enrollment");

        modelBuilder.Entity<Student>().ToTable("Student");
    }
}
