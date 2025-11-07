// This is the main file, it contains cruical components of the program - PoneMaurice

namespace Task;

public static class TaskExtensions
{
	public static void Main()
	{
		int cycle = 0;
		Console.Clear();
		do
		{
			if (cycle == 0)
			{
				Commands.AddFirstProfile();
			}
			Survey.GlobalCommand(Const.PrintInTerminal);
			Commands.AddLog();
			++cycle;
		}
		while (true);
	}
}
