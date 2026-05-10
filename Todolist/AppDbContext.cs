using Microsoft.EntityFrameworkCore;

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
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Login)
                .IsRequired()
                .HasMaxLength(64)
                .UseCollation("NOCASE");

            entity.Property(p => p.Password)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(64);

            entity.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(64);

            entity.Property(p => p.BirthYear)
                .IsRequired();

            entity.HasIndex(p => p.Login).IsUnique();
        });

        modelBuilder.Entity<TodoItem>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Text)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(t => t.Status)
                .HasConversion<int>()
                .IsRequired();

            entity.Property(t => t.LastUpdate)
                .IsRequired();

            entity.Property(t => t.SortOrder)
                .IsRequired();

            entity.Property(t => t.ProfileId)
                .IsRequired();

            entity.HasIndex(t => new { t.ProfileId, t.SortOrder });

            entity.HasOne(t => t.Profile)
                .WithMany(p => p.Todos)
                .HasForeignKey(t => t.ProfileId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
