using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskVerb;

internal class TaskObj
{
	/// <summary>
	/// Asynchronously retrieves all tasks associated with the currently active user profile.
	/// </summary>
	/// <remarks>This method obtains the active user profile before retrieving the associated tasks. Ensure that an
	/// active user profile is set to receive valid results.</remarks>
	/// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
	/// cref="TaskTodo"/> objects representing the tasks of the active user. The collection is empty if the user has no
	/// tasks.</returns>
	protected internal static async Task<IEnumerable<TaskTodo>> GetAllTasksOfActiveUser()
	{
		Database.Profile user = await ActiveProfile.GetActiveProfile();
		return await GetAllTasksOfProfile(user);
	}
	/// <summary>
	/// Asynchronously retrieves all tasks associated with the specified user profile.
	/// </summary>
	/// <remarks>This method queries the database for tasks linked to the user ID of the provided profile. Ensure
	/// that the profile is valid to avoid exceptions.</remarks>
	/// <param name="profile">The user profile for which to retrieve tasks. This parameter must not be null and should represent a valid profile
	/// with a valid user ID.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of TaskTodo
	/// objects that belong to the specified user profile. The collection is empty if no tasks are found.</returns>
	protected internal static async Task<IEnumerable<TaskTodo>> GetAllTasksOfProfile(
		Database.Profile profile)
	{
		using (Todo db = new())
		{
			return db.Tasks.Where(t => t.UserId == profile.UserId);
		}
	}
	/// <summary>
	/// Asynchronously retrieves all task types from the database.
	/// </summary>
	/// <remarks>Ensure that the database context is properly configured before calling this method. The returned
	/// collection reflects the current state of the database at the time of the query.</remarks>
	/// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
	/// cref="TypeOfTask"/> objects representing all task types stored in the database.</returns>
	protected internal static async Task<IEnumerable<TypeOfTask>> GetAllTypeOfTask()
	{
		using (Todo db = new())
		{
			return db.TypesOfTasks;
		}
	}
	/// <summary>
	/// Asynchronously retrieves all available task states from the database.
	/// </summary>
	/// <remarks>Ensure that the database context is properly initialized before calling this method. The returned
	/// collection reflects the current state of the database at the time of the call.</remarks>
	/// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of <see
	/// cref="StateOfTask"/> objects representing the current states of tasks.</returns>
	protected internal static async Task<IEnumerable<StateOfTask>> GetAllStateOfTask()
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
	protected internal static async Task<int> AddNew(
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
	protected internal static async Task<int> AddNew(
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
	protected internal static async Task<Dictionary<int, string>> GetAllStates()
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
	protected internal static async Task<Dictionary<int, string>> GetAllTypes()
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
	/// <summary>
	/// Asynchronously retrieves the type information associated with the specified to-do task.
	/// </summary>
	/// <remarks>This method queries the database for a type of task that matches the TypeId of the provided to-do
	/// task. Ensure that the database context is properly configured and that the task has a valid TypeId. If no matching
	/// type is found, an exception may be thrown.</remarks>
	/// <param name="task">The to-do task for which to retrieve the type information. This parameter must not be null and must have a valid
	/// TypeId.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a TypeOfTask object corresponding to
	/// the specified to-do task.</returns>
	protected internal static async Task<TypeOfTask> GetTypeOfTask(TaskTodo task)
	{
		using (Todo db = new())
		{
			return db.TypesOfTasks.First(t => t.TypeId == task.TypeId);
		}
	}
	/// <summary>
	/// Retrieves the current state associated with the specified task.
	/// </summary>
	/// <remarks>This method queries the database for the state corresponding to the task's StateId. Ensure that the
	/// task has a valid StateId to avoid exceptions.</remarks>
	/// <param name="task">The task for which to retrieve the state. This parameter cannot be null and must have a valid StateId.</param>
	/// <returns>A StateOfTask object representing the current state of the specified task.</returns>
	protected internal static async Task<StateOfTask> GetStateOfTask(TaskTodo task)
	{
		using (Todo db = new())
		{
			return db.StatesOfTask.First(s => s.StateId == task.StateId);
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
	protected internal static async Task<Database.Profile> GetProfileOfTask(TaskTodo task)
	{
		using (Todo db = new())
		{
			return db.Profiles.First(p => p.UserId == task.UserId);
		}
	}
}
