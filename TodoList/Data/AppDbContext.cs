using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Models;
namespace TodoApp.Data
{
	public class AppDbContext : DbContext
	{
		public DbSet<TodoItem> Todos => Set<TodoItem>();
		public DbSet<Profile> Profiles => Set<Profile>();

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
			=> optionsBuilder.UseSqlite("Data Source=todos.db");

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Profile>()
				.HasMany(p => p.Todos)
				.WithOne(t => t.Profile)
				.HasForeignKey(t => t.ProfileId)
				.OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<TodoItem>()
				.Property(t => t.Text)
				.IsRequired()
				.HasMaxLength(500);
		}
	}
}
