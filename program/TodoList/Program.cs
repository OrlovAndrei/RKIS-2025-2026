// This is the main file, it contains cruical components of the program - PoneMaurice
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
				Survey.ParseArgs(args);
				break;
        }
	}
	internal static void Run()
	{
		Commands.AddFirstProfile();
		Console.Clear();
		while (true)
		{
			Main(Input.String(Const.PrintInTerminal).
			Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
			Commands.AddLog();
		}
	}
}
