using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.Task;

internal class Task
{
	public static async Task<IEnumerable<TaskTodo>> GetAllTasks()
	{
		Database.Profile user = await ActiveProfile.GetActiveProfile();
		using (Todo db = new())
		{
			return db.Tasks.Where(t => t.UserId == user.UserId);
		}
	}
	public static async Task<IEnumerable<TypeOfTask>> GetAllTypeOfTask()
	{
		using (Todo db = new())
		{
			return db.TypesOfTasks;
		}
	}
	public static async Task<IEnumerable<StateOfTask>> GetAllStateOfTask()
	{
		using (Todo db = new())
		{
			return db.StatesOfTask;
		}
	}
	/// <summary>
	/// Asynchronously adds a new task to the database and returns the number of state entries written to the database.
	/// </summary>
	/// <remarks>Ensure that the provided task meets all required constraints before calling this method. The method
	/// uses a new database context for each invocation.</remarks>
	/// <param name="newTask">The task to add to the database. This parameter cannot be null.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the number of state entries written to
	/// the database.</returns>
	public static async Task<int> AddNew(
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
	public static async Task<int> AddNew(
		IEnumerable<TaskTodo> newTasks)
	{
		using (Todo db = new())
		{
			await db.Tasks.AddRangeAsync(newTasks);
			return await db.SaveChangesAsync();
		}
	}
	/// <summary>
	/// Asynchronously retrieves all task states, mapping each state ID to its corresponding name.
	/// </summary>
	/// <remarks>This method queries the database for task states. Ensure that the database context is properly
	/// configured and accessible when calling this method.</remarks>
	/// <returns>A dictionary where each key is a state ID and each value is the name of the state.</returns>
	public static async Task<Dictionary<int, string>> GetAllStates()
	{
		using (Todo db = new())
		{
			return (Dictionary<int, string>)
				(from stale in db.StatesOfTask
				 select new
				 {
					 stale.StateId,
					 stale.Name
				 });
		}
	}
	/// <summary>
	/// Asynchronously retrieves all task types from the database and returns a dictionary mapping each type's identifier
	/// to its name.
	/// </summary>
	/// <remarks>Ensure that the database context is properly configured and accessible before calling this method.
	/// The returned dictionary will be empty if no task types are found.</remarks>
	/// <returns>A dictionary where each key is a task type identifier and each value is the corresponding task type name.</returns>
	public static async Task<Dictionary<int, string>> GetAllTypes()
	{
		using (Todo db = new())
		{
			return (Dictionary<int, string>)
				(from type in db.TypesOfTasks
				 select new
				 {
					 type.TypeId,
					 type.Name
				 });
		}
	}
}
