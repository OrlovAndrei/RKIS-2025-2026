//This file contains every command and option for program and their logic
using static Task.Commands;
using static Task.Const;
using static Task.Helpers;
namespace Task;

public static class Survey
{
	public static SearchCommand? CommandLineGlobal { set; get; }
	public static int resultOperation = 0;
	public static void ParseArgs(string[] args)
    {
		SearchCommand commandLine = new(args);
		CommandLineGlobal = commandLine;
		resultOperation = 0;
		switch (commandLine.Command)
		{
			case "add":
				resultOperation = commandLine.Options switch
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
				resultOperation = commandLine.Options switch
				{
					["help"] => ProfileHelp(),
					["change"] => UseActiveProfile(),
					["index"] => FixingIndexing(ProfileName),
					_ => PrintActivePriFile()
				};
				break;
			case "print":
				resultOperation = commandLine.Options switch
				{
					["help"] => PrintHelp(),
					["task"] => PrintAll(TaskName),
					["config"] => PrintAll(commandLine.Argument + OpenFile.PrefConfigFile),
					["profile"] => PrintAll(ProfileName),
					["log"] => PrintAll(LogName),
					["captions"] => WriteCaption(),
					_ => PrintAll(commandLine.Argument)
				};
				break;
			case "search":
				resultOperation = commandLine.Options switch
				{
					["help"] => SearchHelp(),
					["task"] => SearchPartData(TaskName, commandLine.Argument),
					["profile"] => SearchPartData(ProfileName, commandLine.Argument),
					["numbering"] => SearchPartData(commandLine.Argument, null, 0),
					["captions"] => WriteCaption(),
					_ => SearchPartData(commandLine.Argument)
				};
				break;
			case "clear":
				resultOperation = commandLine.Options switch
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
				resultOperation = commandLine.Options switch
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
			case "exit":
				Environment.Exit(0);
				break;
			case "help":
			default:
				Help();
				break;
		}
	}	
}