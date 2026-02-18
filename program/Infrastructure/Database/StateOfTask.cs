namespace ShevricTodo.Database;

public class StateOfTask
{
	public int StateId { get; set; }
	public string? Name { get; set; }
	public string? Description { get; set; }
	public virtual ICollection<TaskTodo>? Tasks { get; set; }
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
			return await db.StatesOfTask.ToArrayAsync();
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
			return await db.StatesOfTask.ToDictionaryAsync
				(s => s.StateId, s => s.Name.NotAvailable());
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
}
