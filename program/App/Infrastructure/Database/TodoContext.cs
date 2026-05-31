using Domain.Entities.ProfileEntity;
using Domain.Entities.TaskEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Database;

public class TodoContext : DbContext
{
	public DbSet<TodoTask> Tasks { get; set; }
	public DbSet<Profile> Profiles { get; set; }
	public DbSet<TaskPriority> TaskPriorities { get; set; }
	public DbSet<TaskState> TaskStates { get; set; }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		string path = CreatePath.PathToDb();
		optionsBuilder.UseSqlite($"Filename={path}");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// Глобальный конвертер для всех Guid свойств
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(Guid))
                {
                    property.SetValueConverter(new GuidToStringConverter());
                    property.SetMaxLength(36);
                }
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TodoContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
	}
}