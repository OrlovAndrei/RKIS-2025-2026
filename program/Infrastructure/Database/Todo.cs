using Microsoft.EntityFrameworkCore;

namespace ShevricTodo.Database;

internal class Todo : DbContext
{
	public DbSet<TaskTodo> Tasks { get; set; }
	public DbSet<Profile> Profiles { get; set; }
	public DbSet<StateOfTask> StatesOfTask { get; set; }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string path = CreatePath.CreatePathToFileInSpecialFolder(
			fileName: $"{nameof(Todo)}.db",
			directory: [ProgramConst.AppName, "Database"]);
		optionsBuilder.UseSqlite($"Filename={path}");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<TaskTodo>()
			.HasOne(t => t.TaskCreator)
			.WithMany(p => p.Tasks)
			.HasForeignKey(t => t.UserId)
			.IsRequired();

		modelBuilder.Entity<TaskTodo>()
			.HasOne(t => t.StateOfTask)
			.WithMany(sof => sof.Tasks)
			.HasForeignKey(t => t.StateId)
			.IsRequired();

		modelBuilder.Entity<Profile>()
			.HasMany(p => p.Tasks)
			.WithOne(t => t.TaskCreator)
			.HasPrincipalKey(t => t.UserId);

		modelBuilder.Entity<StateOfTask>()
			.HasMany(sof => sof.Tasks)
			.WithOne(t => t.StateOfTask)
			.HasPrincipalKey(t => t.StateId);

		StateOfTask notStarted = new()
		{
			StateId = 1,
			Name = "Не начато",
			Description = "Задание существует, но не было начато"
		};

		StateOfTask inProgress = new()
		{
			StateId = 2,
			Name = "В процессе",
			Description = "Задание было начато и находится в процессе выполнения"
		};

		StateOfTask completed = new()
		{
			StateId = 3,
			Name = "Выполнено",
			Description = "Задание успешно выполнено"
		};

		StateOfTask postponed = new()
		{
			StateId = 4,
			Name = "Отложено",
			Description = "Задание было отложено"
		};

		StateOfTask failed = new()
		{
			StateId = 5,
			Name = "Провалено",
			Description = "Задание провалено"
		};

		modelBuilder.Entity<StateOfTask>()
			.HasData(notStarted, inProgress, completed, postponed, failed);

		modelBuilder.Entity<TaskTodo>()
			.HasKey(t => t.TaskId);

		modelBuilder.Entity<Profile>()
			.HasKey(p => p.UserId);

		modelBuilder.Entity<StateOfTask>()
			.HasKey(sof => sof.StateId);

		modelBuilder.Entity<TaskTodo>()
			.Property(t => t.TaskId)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<Profile>()
			.Property(p => p.UserId)
			.ValueGeneratedOnAdd();

		modelBuilder.Entity<StateOfTask>()
			.Property(sof => sof.StateId)
			.ValueGeneratedOnAdd();
	}
}