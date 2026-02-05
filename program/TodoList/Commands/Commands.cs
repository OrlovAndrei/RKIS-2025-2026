
namespace TodoList;

public partial class Commands
{
	public static int ConsoleClear()
	{
		Console.Clear();
		return 1;
	}
	public static int Exit()
	{
		Environment.Exit(0);
		return 1;
    }
}
