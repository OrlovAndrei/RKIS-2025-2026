using Microsoft.EntityFrameworkCore;
using TodoList.Models;

namespace TodoList.Data;

public class AppDbContext : DbContext
{
	public DbSet<TodoItem> Todos => Set<TodoItem>();
	public DbSet<Profile> Profiles => Set<Profile>();

	protected override void OnConfiguring(DbContextOptionsBuilder o)
		=> o.UseSqlite("Data Source=todos.db");

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<TodoItem>()
			.HasOne(t => t.Profile)
			.WithMany(p => p.TodoItems)
			.HasForeignKey(t => t.ProfileId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}