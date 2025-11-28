using Microsoft.EntityFrameworkCore;
using RESTfull.Domain.Entities;

namespace RESTfull.Infrastructure.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Education> Educations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.LastName).IsRequired().HasMaxLength(100);
                entity.Property(s => s.Patronymic).HasMaxLength(100);
                entity.Property(s => s.StudentCardNumber).IsRequired().HasMaxLength(50);
                entity.Property(s => s.RegistrationDate).IsRequired();
            });

            modelBuilder.Entity<Education>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Institution).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Faculty).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Specialty).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Profile).HasMaxLength(100);
                entity.Property(e => e.Form).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Qualification).HasMaxLength(100);
                entity.Property(e => e.Group).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);

                entity.HasOne(e => e.Student)
                      .WithMany(s => s.Educations)
                      .HasForeignKey(e => e.StudentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}