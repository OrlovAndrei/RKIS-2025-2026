namespace TodoList;

public class Program
{
	public static void Main(string[] args)
	{
		switch (args.Length)
		{
			case 0:
				Run();
				break;
			default:
				RunNotCycle(args);
				break;
		}
	}
	internal static void RunNotCycle(string[] args)
	{
		Commands.ProfileInitialization();
		Survey.ParseArgs(args);
		Commands.AddLog();
	}
	internal static void Run()
	{
		Console.Clear();
		Commands.ProfileInitialization();
		while (true)
		{
			Main(Input.String(Const.PrintInTerminal).
				Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
		}
	}
}
