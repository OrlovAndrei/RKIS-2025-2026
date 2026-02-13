namespace ShevricTodo.Commands;

internal static class Exit
{
	public static async Task Done() => Program.RunRunRun = false;
}
