using Microsoft.EntityFrameworkCore;
using OneToManyAPI_Demo.Models;

namespace OneToManyAPI_Demo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){ }

        public DbSet<Department> departments { get; set; }
        public DbSet<Employee> employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Department>()
                .HasMany(e => e.Employees)
                .WithOne(d => d.Department)
                .HasForeignKey(fk => fk.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
