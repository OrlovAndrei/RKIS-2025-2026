using Infrastructure.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class TodoContext : DbContext
{
	public DbSet<TodoTaskEntity> Tasks { get; set; }
	public DbSet<ProfileEntity> Profiles { get; set; }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string path = CreatePath.PathToDb();
		optionsBuilder.UseSqlite($"Filename={path}");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Задача

		modelBuilder.Entity<TodoTaskEntity>()
			.HasKey(t => t.TaskId);

		modelBuilder.Entity<TodoTaskEntity>()
			.HasOne(t => t.TaskCreator)
			.WithMany(p => p.Tasks)
			.HasForeignKey(t => t.ProfileId);

		modelBuilder.Entity<TodoTaskEntity>()
			.Property(t => t.TaskId)
			.IsRequired();

		modelBuilder.Entity<TodoTaskEntity>()
			.Property(t => t.StateId)
			.IsRequired();

		modelBuilder.Entity<TodoTaskEntity>()
			.Property(t => t.PriorityLevel)
			.IsRequired();

		modelBuilder.Entity<TodoTaskEntity>()
			.Property(t => t.ProfileId)
			.IsRequired();

		modelBuilder.Entity<TodoTaskEntity>()
			.Property(t => t.Name)
			.IsRequired();

		modelBuilder.Entity<TodoTaskEntity>()
			.Property(t => t.CreateAt)
			.IsRequired();

		// Профиль

		modelBuilder.Entity<ProfileEntity>()
			.HasKey(p => p.ProfileId);

		modelBuilder.Entity<ProfileEntity>()
			.HasMany(p => p.Tasks)
			.WithOne(t => t.TaskCreator)
			.HasPrincipalKey(p => p.ProfileId);

		modelBuilder.Entity<ProfileEntity>()
			.Property(p => p.ProfileId)
			.IsRequired();

		modelBuilder.Entity<ProfileEntity>()
			.Property(p => p.FirstName)
			.IsRequired();

		modelBuilder.Entity<ProfileEntity>()
			.Property(p => p.LastName)
			.IsRequired();

		modelBuilder.Entity<ProfileEntity>()
			.Property(p => p.CreatedAt)
			.IsRequired();

		modelBuilder.Entity<ProfileEntity>()
			.Property(p => p.DateOfBirth)
			.IsRequired();

		modelBuilder.Entity<ProfileEntity>()
			.Property(p => p.PasswordHash)
			.IsRequired();
	}
}