using Microsoft.EntityFrameworkCore;
using GradeManagement.Entity;

namespace GradeManagement.Config
{
  public class AppDbContext : DbContext
  {
    // DbSets for all entities
    public DbSet<User> Users { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<FManager> FManagers { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Faculty> Faculties { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Result> Results { get; set; }


    // Constructor
    public AppDbContext() : base()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Configure connection string
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      if (!optionsBuilder.IsConfigured)
      {
        var connectionString = DatabaseConfig.GetConnectionString();
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 33));
        optionsBuilder.UseMySql(connectionString, serverVersion);
      }
    }
    
    // Configure entity relationships and Table Per Type (TPT) inheritance
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Configure Table Per Type (TPT) inheritance for User hierarchy
      modelBuilder.Entity<User>()
        .ToTable("user")
        .HasKey(u => u.id);

      modelBuilder.Entity<Student>()
        .ToTable("student")
        .HasBaseType<User>()
        .HasOne(f => f.Faculty)
        .WithMany()
        .HasForeignKey("fid");

      modelBuilder.Entity<FManager>()
        .ToTable("fManager")
        .HasBaseType<User>()
        .HasOne(f => f.Faculty)
        .WithMany()
        .HasForeignKey("fid");

      modelBuilder.Entity<Admin>()
        .ToTable("admin")
        .HasBaseType<User>();

      modelBuilder.Entity<Faculty>()
        .ToTable("faculty")
        .HasKey(f => f.id);

      // Configure Course entity
      modelBuilder.Entity<Course>()
        .ToTable("course")
        .HasKey(c => c.id);

      // Configure Result entity with composite key and foreign keys
      modelBuilder.Entity<Result>()
        .ToTable("result")
        .HasKey(r => new { r.sid, r.cid });

      // Configure foreign key relationship with Student
      modelBuilder.Entity<Result>()
        .HasOne(r => r.Student)
        .WithMany()
        .HasForeignKey(r => r.sid)
        .OnDelete(DeleteBehavior.Cascade);

      // Configure foreign key relationship with Course
      modelBuilder.Entity<Result>()
        .HasOne(r => r.Course)
        .WithMany()
        .HasForeignKey(r => r.cid)
        .OnDelete(DeleteBehavior.Cascade);

      // Configure enum to string conversion for Role
      modelBuilder.Entity<User>()
        .Property(u => u.role)
        .HasConversion<string>();
    }
  }
}
