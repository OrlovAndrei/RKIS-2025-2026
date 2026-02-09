using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskObj;

internal partial class List
{
	public static async Task PrintTasks(
		IEnumerable<TaskTodo> tasks)
	{
		await PrintTasks(tasks: tasks, printTable: Input.WriteToConsole.PrintTable);
	}
	public static async Task PrintAllTasksOfActiveUser()
	{
		await PrintAllTasksOfActiveUser(printTable: Input.WriteToConsole.PrintTable);
	}
	public static async Task PrintAllTasksOfProfile(
		Profile profile)
	{
		await PrintAllTasksOfProfile(profile: profile, printTable: Input.WriteToConsole.PrintTable);
	}
}
