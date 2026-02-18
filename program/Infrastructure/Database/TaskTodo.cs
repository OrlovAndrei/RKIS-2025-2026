using ShevricTodo.Authentication;

namespace ShevricTodo.Database;

public class TaskTodo
{
	public int TaskId { get; set; }
	public int TypeId { get; set; }
	public int StateId { get; set; }
	public int UserId { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public DateTime? DateOfCreate { get; set; }
	public DateTime? DateOfStart { get; set; }
	public DateTime? DateOfEnd { get; set; }
	public DateTime? Deadline { get; set; }
	public virtual Profile? TaskCreator { get; set; }
	public virtual StateOfTask? StateOfTask { get; set; }
	/// <summary>
	/// Asynchronously adds a new task to the database and returns the number of state entries written to the database.
	/// </summary>
	/// <remarks>Ensure that the provided task meets all required constraints before calling this method. The method
	/// uses a new database context for each invocation.</remarks>
	/// <param name="newTask">The task to add to the database. This parameter cannot be null.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to
	/// the database.</returns>
	protected internal static async Task<int> Add(
		TaskTodo newTask)
	{
		using (Todo db = new())
		{
			await db.Tasks.AddAsync(newTask);
			return await db.SaveChangesAsync();
		}
	}
	/// <summary>
	/// Asynchronously adds a collection of tasks to the database.
	/// </summary>
	/// <remarks>This method uses a database context to add the specified tasks and save changes. Ensure that the
	/// database is properly configured and accessible before calling this method.</remarks>
	/// <param name="newTasks">An enumerable collection of <see cref="TaskTodo"/> objects representing the tasks to add. Each task must be valid
	/// and not null.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to
	/// the database.</returns>
	protected internal static async Task<int> Add(
		IEnumerable<TaskTodo> newTasks)
	{
		using (Todo db = new())
		{
			await db.Tasks.AddRangeAsync(newTasks);
			return await db.SaveChangesAsync();
		}
	}
	protected internal static async Task<int> Remove(
		TaskTodo task)
	{
		using (Todo db = new())
		{
			db.Tasks.Remove(task);
			return await db.SaveChangesAsync();
		}
	}
	protected internal static async Task<int> Remove(
		IEnumerable<TaskTodo> tasks)
	{
		using (Todo db = new())
		{
			db.Tasks.RemoveRange(tasks);
			return await db.SaveChangesAsync();
		}
	}
	protected internal static async Task<int> Update(
		TaskTodo task)
	{
		using (Todo db = new())
		{
			db.Tasks.Update(task);
			return await db.SaveChangesAsync();
		}
	}
	protected internal static async Task<int> Update(
		IEnumerable<TaskTodo> tasks)
	{
		using (Todo db = new())
		{
			db.Tasks.UpdateRange(tasks);
			return await db.SaveChangesAsync();
		}
	}
	/// <summary>
	/// Retrieves the user profile associated with the specified task.
	/// </summary>
	/// <remarks>This method queries the database for a profile matching the UserId of the provided task. If no
	/// matching profile exists, an exception is thrown.</remarks>
	/// <param name="task">The task for which to retrieve the associated user profile. The task must have a valid UserId.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the ProfileVerb object associated with the
	/// specified task.</returns>
	protected internal static async Task<Profile> GetProfileOfTask(TaskTodo task)
	{
		using (Todo db = new())
		{
			return db.Profiles.First(p => p.UserId == task.UserId);
		}
	}
}
