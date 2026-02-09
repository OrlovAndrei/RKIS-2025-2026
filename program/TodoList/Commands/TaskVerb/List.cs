using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskVerb;

internal class List : TaskObj
{
	/// <summary>
	/// Prints a formatted table of tasks, including associated user, task type, and task state information.
	/// </summary>
	/// <remarks>This method retrieves all task types, user profiles, and task states from the database to ensure
	/// that each task is displayed with complete contextual information. The table includes user details and task
	/// metadata, providing a comprehensive overview of the tasks. The method does not return any data; it performs the
	/// print operation asynchronously.</remarks>
	/// <param name="printTable">An action that prints the table, accepting an array of column headers, an enumerable collection of row values, and
	/// an optional string for additional formatting.</param>
	/// <param name="tasks">An enumerable collection of TaskTodo objects representing the tasks to be displayed in the table.</param>
	/// <returns>A task that represents the asynchronous operation of printing the tasks.</returns>
	private static async Task PrintTasks(
		Func<string[], IEnumerable<string[]>, string?, Task> printTable,
		IEnumerable<TaskTodo> tasks)
	{
		IEnumerable<TypeOfTask> allTypes = await GetAllTypeOfTask();
		IEnumerable<Database.Profile> allProfile = await ProfileVerb.ProfileObj.GetAllProfile();
		IEnumerable<StateOfTask> allStates = await GetAllStateOfTask();
		string[] columns = [
			"TaskId",
			"FirstName",
			"LastName",
			"UserName",
			"TapeOfTask",
			"StateOfTask",
			"Name",
			"Description",
			"DataOfCreate",
			"DateOfStart",
			"DataOfEnd",
			"Deadline"];
		IEnumerable<string[]> rows =
			from task in tasks
			join profile in allProfile on task.UserId equals profile.UserId
			join type in allTypes on task.TypeId equals type.TypeId
			join state in allStates on task.StateId equals state.StateId
			orderby task.TaskId
			select new string[]
			{
				task.TaskId.ToString() ?? "N/A",
				profile.FirstName ?? "N/A",
				profile.LastName ?? "N/A",
				profile.UserName ?? "N/A",
				type.Name ?? "N/A",
				state.Name ?? "N/A",
				task.Name ?? "N/A",
				task.Description ?? "N/A",
				task.DateOfCreate.ToString() ?? "N/A",
				task.DateOfStart.ToString() ?? "N/A",
				task.DateOfEnd.ToString() ?? "N/A",
				task.Deadline.ToString() ?? "N/A"
			}
			.ToArray();
		await printTable(columns, rows, null);
	}
	public static async Task PrintTasks(
		IEnumerable<TaskTodo> tasks)
	{
		await PrintTasks(tasks: tasks, printTable: Input.WriteToConsole.PrintTable);
	}
	/// <summary>
	/// Asynchronously retrieves and displays all tasks associated with the currently active user in a tabular format.
	/// </summary>
	/// <remarks>This method gathers all tasks, task types, and task states for the active user profile and formats
	/// them for display. The active user profile must be available before calling this method.</remarks>
	/// <param name="printTable">An action that renders the table of tasks. Receives an array of column headers, an enumerable collection of row
	/// values, and an optional title for the table.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	private static async Task PrintAllTasksOfActiveUser(
		Func<string[], IEnumerable<string[]>, string?, Task> printTable)
	{
		IEnumerable<TaskTodo> allTasks = await GetAllTasksOfActiveUser();
		IEnumerable<TypeOfTask> allTypes = await GetAllTypeOfTask();
		IEnumerable<StateOfTask> allStates = await GetAllStateOfTask();
		Profile activeUser = await ActiveProfile.GetActiveProfile();
		string title = $"{activeUser.UserId}: {activeUser.FirstName} {activeUser.LastName}";
		string[] columns = [
			"TaskId",
			"TapeOfTask",
			"StateOfTask",
			"Name",
			"Description",
			"DataOfCreate",
			"DateOfStart",
			"DataOfEnd",
			"Deadline"];
		IEnumerable<string[]> rows =
			from task in allTasks
			join type in allTypes on task.TypeId equals type.TypeId
			join state in allStates on task.StateId equals state.StateId
			orderby task.TaskId
			select new string[]
			{
				task.TaskId.ToString() ?? "N/A",
				type.Name ?? "N/A",
				state.Name ?? "N/A",
				task.Name ?? "N/A",
				task.Description ?? "N/A",
				task.DateOfCreate.ToString() ?? "N/A",
				task.DateOfStart.ToString() ?? "N/A",
				task.DateOfEnd.ToString() ?? "N/A",
				task.Deadline.ToString() ?? "N/A"
			}
			.ToArray();
		await printTable(columns, rows, title);
	}
	public static async Task PrintAllTasksOfActiveUser()
	{
		await PrintAllTasksOfActiveUser(printTable: Input.WriteToConsole.PrintTable);
	}
	/// <summary>
	/// Asynchronously retrieves and displays all tasks associated with the specified user profile in a tabular format.
	/// </summary>
	/// <remarks>The method gathers all tasks, their types, and states for the given profile and formats them for
	/// display. The output includes task details such as type, state, name, description, creation date, start date, end
	/// date, and deadline. The profile parameter should refer to an existing user profile.</remarks>
	/// <param name="printTable">An action that renders the table of tasks, accepting an array of column headers, a collection of row values, and an
	/// optional title for the table.</param>
	/// <param name="profile">The user profile for which to retrieve and display tasks. The profile must be valid and active.</param>
	/// <returns>A task that represents the asynchronous operation of retrieving and printing the tasks. The task does not return a
	/// value.</returns>
	private static async Task PrintAllTasksOfProfile(
		Func<string[], IEnumerable<string[]>, string?, Task> printTable,
		Database.Profile profile)
	{
		IEnumerable<TaskTodo> allTasks = await GetAllTasksOfProfile(profile);
		IEnumerable<TypeOfTask> allTypes = await GetAllTypeOfTask();
		IEnumerable<StateOfTask> allStates = await GetAllStateOfTask();
		Database.Profile activeUser = await ActiveProfile.GetActiveProfile();
		string title = $"{profile.UserId}: {profile.FirstName} {profile.LastName}";
		string[] columns = [
			"TaskId",
			"TapeOfTask",
			"StateOfTask",
			"Name",
			"Description",
			"DataOfCreate",
			"DateOfStart",
			"DataOfEnd",
			"Deadline"];
		IEnumerable<string[]> rows =
			from task in allTasks
			join type in allTypes on task.TypeId equals type.TypeId
			join state in allStates on task.StateId equals state.StateId
			orderby task.TaskId
			select new string[]
			{
				task.TaskId.ToString() ?? "N/A",
				type.Name ?? "N/A",
				state.Name ?? "N/A",
				task.Name ?? "N/A",
				task.Description ?? "N/A",
				task.DateOfCreate.ToString()  ?? "N/A",
				task.DateOfStart.ToString() ?? "N/A",
				task.DateOfEnd.ToString() ?? "N/A",
				task.Deadline.ToString() ?? "N/A"
			}
			.ToArray();
		await printTable(columns, rows, title);
	}
	public static async Task PrintAllTasksOfProfile(
		Profile profile)
	{
		await PrintAllTasksOfProfile(profile: profile, printTable: Input.WriteToConsole.PrintTable);
	}
}
