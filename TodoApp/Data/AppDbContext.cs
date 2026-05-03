using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TodoItem> Todos => Set<TodoItem>();
        public DbSet<Profile> Profiles => Set<Profile>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=todos.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(profile => profile.Id);
                entity.HasIndex(profile => profile.Login).IsUnique();

                entity.Property(profile => profile.Login)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(profile => profile.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(profile => profile.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(profile => profile.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(profile => profile.BirthYear)
                    .IsRequired();

                entity.HasMany(profile => profile.Todos)
                    .WithOne(todo => todo.Profile)
                    .HasForeignKey(todo => todo.ProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(todo => todo.Id);

                entity.Property(todo => todo.Text)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(todo => todo.Status)
                    .IsRequired();

                entity.Property(todo => todo.LastUpdate)
                    .IsRequired();
            });
        }
    }
}
