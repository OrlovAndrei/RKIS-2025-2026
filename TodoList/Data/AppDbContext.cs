using Microsoft.EntityFrameworkCore;

namespace TodoList.Data;

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
        modelBuilder.Entity<Profile>()
            .HasMany(p => p.TodoItems)
            .WithOne(t => t.Profile)
            .HasForeignKey(t => t.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasIndex(p => p.Login).IsUnique();
            entity.Property(p => p.Login).IsRequired().HasMaxLength(50);
            entity.Property(p => p.Password).IsRequired().HasMaxLength(100);
            entity.Property(p => p.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(p => p.LastName).IsRequired().HasMaxLength(50);
            entity.Property(p => p.BirthYear).IsRequired();
        });

        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.Property(t => t.Text).IsRequired();
            entity.Property(t => t.Status).IsRequired().HasConversion<string>();
            entity.Property(t => t.LastUpdate).IsRequired();
        });
    }
}