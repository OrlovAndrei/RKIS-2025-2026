using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data;

public class AppDbContext : DbContext
{
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<TodoItem> Todos => Set<TodoItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=todo.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>()
            .HasOne(t => t.Profile)
            .WithMany(p => p.Todos)
            .HasForeignKey(t => t.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Profile>()
            .HasIndex(p => p.Login)
            .IsUnique();
    }
}