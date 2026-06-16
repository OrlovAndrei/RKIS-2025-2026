using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TodoList.Database;

public class DatabaseInitialization
{
	private readonly DbContext _context;
	public DatabaseInitialization(DbContext context)
	{
		_context = context;
	}
	public async Task InitializeAsync()
	{
		var migrationsAssembly = _context.GetService<IMigrationsAssembly>();
		var migrations = migrationsAssembly.Migrations; // Список миграций
		if (await _context.Database.EnsureCreatedAsync())
		{
			// Seed initial data if necessary
		}
		else if (migrations.Any() && _context.Database.HasPendingModelChanges())
		{
			throw new InvalidOperationException(
@"The database schema is out of date. Please apply migrations before running the application.
    Command to create migration: dotnet ef migrations add <MigrationName>.
    Command to apply migration: dotnet ef database update.");
		}
		// else
		// {
		// 	await _context.Database.MigrateAsync();
		// }
	}
}