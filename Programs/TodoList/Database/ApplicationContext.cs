using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoList.Entity;
using TodoList.Interfaces;

namespace TodoList.Database;

public class ApplicationContext : DbContext
{
    private readonly IConnectionStrategy _connectionStrategy;
    public DbSet<TodoItem> Tasks { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public ApplicationContext(IConnectionStrategy connectionStrategy)
    {
        _connectionStrategy = connectionStrategy;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        _connectionStrategy.Configure(optionsBuilder);
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

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}