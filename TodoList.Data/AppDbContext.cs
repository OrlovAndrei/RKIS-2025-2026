using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<TodoItem> Todos => Set<TodoItem>();
        public DbSet<Profile> Profiles => Set<Profile>();
        public DbSet<User> Users => Set<User>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=todos.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .HasOne(t => t.Profile)
                .WithMany(p => p.Todos)
                .HasForeignKey(t => t.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Text).IsRequired().HasMaxLength(500);
                entity.Property(t => t.Status).IsRequired();
                entity.Property(t => t.LastUpdate).IsRequired();
                entity.Property(t => t.ProfileId).IsRequired();
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Login).IsRequired().HasMaxLength(50);
                entity.Property(p => p.Password).IsRequired().HasMaxLength(100);
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(p => p.LastName).IsRequired().HasMaxLength(50);
                entity.Property(p => p.BirthYear).IsRequired();

                entity.HasIndex(p => p.Login).IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(user => user.Id);
                entity.Property(user => user.Username).IsRequired().HasMaxLength(50);
                entity.Property(user => user.Email).IsRequired().HasMaxLength(100);
                entity.Property(user => user.PasswordHash).IsRequired().HasMaxLength(512);
                entity.Property(user => user.Role).IsRequired().HasMaxLength(30);
                entity.Property(user => user.ProfileId).IsRequired();

                entity.HasIndex(user => user.Username).IsUnique();
                entity.HasIndex(user => user.Email).IsUnique();
                entity.HasOne(user => user.Profile)
                    .WithOne()
                    .HasForeignKey<User>(user => user.ProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
