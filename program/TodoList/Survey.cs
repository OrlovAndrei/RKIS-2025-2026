//This file contains every command and option for program and their logic
using static Task.Commands;
using static Task.Const;
using static Task.Helpers;
namespace Task;

public class Survey
{
	public static SearchCommand? CommandLineGlobal { set; get; }
	public static int ResultOperation { set; get; } = 0;
	public void GlobalCommand(string text)
	{
		string ask = Input.String(text);
		SearchCommand commandLine = new(ask.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
		CommandLineGlobal = commandLine;
		switch (commandLine.Command)
		{
			case "add":
				ResultOperation = commandLine.Options switch
				{
					["help"] => AddHelp(),
					["task"] => AddTask(),
					["multi", "task"] => MultiAddTask(),
					["task", "print"] => AddTaskAndPrint(),
					["config"] => AddConfUserData(commandLine.Argument),
					["profile"] => AddProfile(),
					_ => AddUserData(commandLine.Argument)
				};
				break;
			case "profile":
				ResultOperation = commandLine.Options switch
				{
					["help"] => ProfileHelp(),
					["change"] => UseActiveProfile(),
					["index"] => FixingIndexing(ProfileName),
					_ => PrintActivePriFile()
				};
				break;
			case "print":
				ResultOperation = commandLine.Options switch
				{
					["help"] => PrintHelp(),
					["task"] => PrintAll(TaskName),
					["bool"] => PrintSpecific("Bool", TaskName),
					["desc"] => PrintSpecific("description", TaskName),
					["date"] => PrintSpecific("deadLine", TaskName),
					["config"] => PrintAll(commandLine.Argument + PrefConfigFile),
					["profile"] => PrintAll(ProfileName),
					["log"] => PrintAll(LogName),
					["captions"] => WriteCaption(),
					_ => PrintAll(commandLine.Argument)
				};
				break;
			case "search":
				ResultOperation = commandLine.Options switch
				{
					["help"] => SearchHelp(),
					["task"] => SearchPartData(TaskName, commandLine.Argument),
					["profile"] => SearchPartData(ProfileName, commandLine.Argument),
					["numbering"] => 0, ////////////////////////////////////////////////////////////////////////
					["captions"] => WriteCaption(),
					_ => SearchPartData(commandLine.Argument)
				};
				break;
			case "clear":
				ResultOperation = commandLine.Options switch
				{
					["help"] => ClearHelp(),
					["task"] => ClearRow(TaskName, commandLine.Argument),
					["task", "all"] => ClearAllFile(TaskName),
					["profile"] => ClearRow(ProfileName, commandLine.Argument),
					["profile", "all"] => ClearAllFile(ProfileName),
					["console"] => ConsoleClear(),
					["all"] => ClearAllFile(commandLine.Argument),
					_ => ClearRow(commandLine.Argument)
				};
				break;
			case "edit":
				ResultOperation = commandLine.Options switch
				{
					["help"] => EditHelp(),
					["task"] => EditRow(TaskName, commandLine.Argument),
					["task", "index"] => FixingIndexing(TaskName),
					["task", "bool"] => EditBoolRow(TaskName, commandLine.Argument),
					["bool"] => EditBoolRow(commandLine.Argument),
					["index"] => FixingIndexing(commandLine.Argument),
					["all"] => ClearAllFile(commandLine.Argument),
					_ => EditRow(commandLine.Argument)
				};
				break;
			case "help":
				Help();
				break;
			case "exit":
				Environment.Exit(0);
				break;
			default:
				WriteToConsole.RainbowText("Такой команды не существует", ConsoleColor.Red);
				break;
		}

	}
}