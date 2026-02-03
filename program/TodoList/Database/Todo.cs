using Microsoft.EntityFrameworkCore;

namespace ShevricTodo.Database;

internal class Todo : DbContext
{
	public DbSet<TaskTodo> Tasks { get; set; }
	public DbSet<Profile> Profiles { get; set; }
	public DbSet<TypeOfTask> TypeOfTasks { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string path = CreatePath.CreatePathToFileInSpecialFolder(
			fileName: "Todo.db",
			directory: ["ShevricTodo", "Database"]);
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
			.HasOne(t => t.TypeOfTask)
			.WithMany(tof => tof.Tasks)
			.HasForeignKey(t => t.TypeId)
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
		modelBuilder.Entity<TypeOfTask>()
			.HasMany(tof => tof.Tasks)
			.WithOne(t => t.TypeOfTask)
			.HasPrincipalKey(t => t.TypeId);
		modelBuilder.Entity<StateOfTask>()
			.HasMany(sof => sof.Tasks)
			.WithOne(t => t.StateOfTask)
			.HasPrincipalKey(t => t.StateId);
		modelBuilder.Entity<TaskTodo>()
			.HasKey(t => t.TaskId);
		modelBuilder.Entity<Profile>()
			.HasKey(p => p.UserId);
		modelBuilder.Entity<TypeOfTask>()
			.HasKey(tof => tof.TypeId);
		modelBuilder.Entity<StateOfTask>()
			.HasKey(sof => sof.StateId);
	}
}