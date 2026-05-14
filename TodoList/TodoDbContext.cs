using Microsoft.EntityFrameworkCore;

public class TodoDbContext : DbContext
{
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<TodoItem> Todos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=todo.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Login).IsRequired().HasMaxLength(100);
            entity.HasIndex(p => p.Login).IsUnique();
            entity.Property(p => p.Password).IsRequired();
            entity.Property(p => p.FirstName).IsRequired();
            entity.Property(p => p.LastName).IsRequired();
            entity.Property(p => p.BirthYear).IsRequired();

            entity.HasMany(p => p.Todos)
                  .WithOne(t => t.Profile)
                  .HasForeignKey(t => t.ProfileId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Text).IsRequired();
            entity.Property(t => t.Status).IsRequired();
            entity.Property(t => t.LastUpdate).IsRequired();
            entity.Property(t => t.ProfileId).IsRequired();
        });
    }
}