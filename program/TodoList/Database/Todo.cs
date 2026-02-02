using Microsoft.EntityFrameworkCore;

namespace ShevricTodo.Database;

internal class Todo : DbContext
{
	public DbSet<Task> Tasks { get; set; }
	public DbSet<Profile> Profiles { get; set; }
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		
	}
}
