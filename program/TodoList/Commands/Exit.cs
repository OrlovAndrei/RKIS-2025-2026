namespace ShevricTodo.Commands;

internal class Exit
{
	public static async Task Done()
	{
		Program.RunRunRun = false;
	}
}
