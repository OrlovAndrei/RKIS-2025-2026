using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public class TodoContext : DbContext
{
	public DbSet<TodoTaskEntity> Tasks { get; set; }
	public DbSet<ProfileEntity> Profiles { get; set; }
	public DbSet<TaskStateEntity> StatesOfTask { get; set; }
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
			.HasOne(t => t.StateOfTask)
			.WithMany(sof => sof.Tasks)
			.HasForeignKey(t => t.StateId);

		modelBuilder.Entity<TodoTaskEntity>()
			.Property(t => t.TaskId)
			.IsRequired();

		modelBuilder.Entity<TodoTaskEntity>()
			.Property(t => t.StateId)
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

		// Состояние задачи

		modelBuilder.Entity<TaskStateEntity>()
			.HasKey(sof => sof.StateId);

		modelBuilder.Entity<TaskStateEntity>()
			.HasMany(sof => sof.Tasks)
			.WithOne(t => t.StateOfTask)
			.HasPrincipalKey(sof => sof.StateId);

		modelBuilder.Entity<TaskStateEntity>()
			.Property(sof => sof.StateId)
			.IsRequired();

		modelBuilder.Entity<TaskStateEntity>()
			.Property(sof => sof.Name)
			.IsRequired();

		modelBuilder.Entity<TaskStateEntity>()
			.Property(sof => sof.IsCompleted)
			.IsRequired();

		TaskStateEntity notStarted = new()
		{
			StateId = Guid.NewGuid().ToString(),
			Name = "Не начато",
			Description = "Задание существует, но не было начато"
		};

		TaskStateEntity inProgress = new()
		{
			StateId = Guid.NewGuid().ToString(),
			Name = "В процессе",
			Description = "Задание было начато и находится в процессе выполнения"
		};

		TaskStateEntity completed = new()
		{
			StateId = Guid.NewGuid().ToString(),
			Name = "Выполнено",
			Description = "Задание успешно выполнено"
		};

		TaskStateEntity postponed = new()
		{
			StateId = Guid.NewGuid().ToString(),
			Name = "Отложено",
			Description = "Задание было отложено"
		};

		TaskStateEntity failed = new()
		{
			StateId = Guid.NewGuid().ToString(),
			Name = "Провалено",
			Description = "Задание провалено"
		};

		modelBuilder.Entity<TaskStateEntity>()
			.HasData(notStarted, inProgress, completed, postponed, failed);
	}
}