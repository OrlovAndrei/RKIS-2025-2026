// This is the main file, it contains cruical components of the program - PoneMaurice
namespace Task;

public class Program
{
	public static void Main(string[] args)
	{
		if (args.Length == 0)
		{
			Run();
		}
        else
        {
            Survey.ParseArgs(args);
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
