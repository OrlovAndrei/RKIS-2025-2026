namespace ShevricTodo.Commands.TaskObj;

internal partial class Show
{
	public static async Task ShowTask(
		Database.TaskTodo task)
	{
		await ShowTask(task: task, printPanel: Input.WriteToConsole.PrintPanel);
	}
}
