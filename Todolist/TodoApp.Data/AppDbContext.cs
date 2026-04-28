using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Profile> Profiles => Set<Profile>();

    public DbSet<TodoItem> Todos => Set<TodoItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=todos.db");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(profile => profile.Id);

            entity.Property(profile => profile.Login)
                .IsRequired()
                .HasMaxLength(64)
                .UseCollation("NOCASE");

            entity.Property(profile => profile.Password)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(profile => profile.FirstName)
                .IsRequired()
                .HasMaxLength(64);

            entity.Property(profile => profile.LastName)
                .IsRequired()
                .HasMaxLength(64);

            entity.Property(profile => profile.BirthYear)
                .IsRequired();

            entity.Ignore(profile => profile.Age);
            entity.Ignore(profile => profile.DisplayName);
            entity.HasIndex(profile => profile.Login).IsUnique();
        });

        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(todo => todo.Id);

            entity.Property(todo => todo.Text)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(todo => todo.Status)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(todo => todo.LastUpdate)
                .IsRequired();

            entity.Property(todo => todo.SortOrder)
                .IsRequired();

            entity.Property(todo => todo.ProfileId)
                .IsRequired();

            entity.Ignore(todo => todo.IsCompleted);
            entity.HasIndex(todo => new { todo.ProfileId, todo.SortOrder });

            entity.HasOne(todo => todo.Profile)
                .WithMany(profile => profile.Todos)
                .HasForeignKey(todo => todo.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
