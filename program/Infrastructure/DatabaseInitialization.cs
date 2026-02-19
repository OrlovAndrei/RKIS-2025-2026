using Application.Interfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class DatabaseInitialization : IDatabaseInitialization
{
    private readonly TodoContext _context;
    public DatabaseInitialization(TodoContext context)
    {
        _context = context;
    }
	public void Initialize()
	{
		if (_context.Database.EnsureCreated())
        {
            // Seed initial data if necessary
        }
//         else if (_context.Database.HasPendingModelChanges())
//         {
//             throw new InvalidOperationException(
// @"The database schema is out of date. Please apply migrations before running the application.
//     Command to create migration: dotnet ef migrations add <MigrationName>.
//     Command to apply migration: dotnet ef database update.");
//         }
        else
        {
            _context.Database.Migrate();
        }
	}
}