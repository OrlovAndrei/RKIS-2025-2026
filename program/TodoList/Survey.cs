//This file contains every command and option for program and their logic
using static Task.Commands;
namespace Task;

public class Survey
{
	public static SearchCommandOnJson? commandLineGlobal;
	public int resultOperation = 0;
	public void GlobalCommand(string text)
	{
		string ask = Input.String(text);
		SearchCommandOnJson commandLine = new(ask.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
		commandLineGlobal = commandLine;
		switch (commandLine.commandOut)
		{
			case "add":
				resultOperation = commandLine.optionsOut switch
				{
					["help"] => AddHelp(),
					["task"] => AddTask(),
					["multi", "task"] => MultiAddTask(),
					["task", "print"] => AddTaskAndPrint(),
					["config"] => AddConfUserData(commandLine.nextTextOut),
					["profile"] => AddProfile(),
					_ => AddUserData(commandLine.nextTextOut)
				};
				break;
			case "profile":
				resultOperation = commandLine.optionsOut switch
				{
					["help"] => ProfileHelp(),
					["change"] => UseActiveProfile(),
					["index"] => FixingIndexing(Const.ProfileName),
					_ => PrintActivePriFile()
				};
				break;
			case "print":
				resultOperation = commandLine.optionsOut switch
				{
					["help"] => PrintHelp(),
					["task"] => PrintAll(Const.TaskName),
					["config"] => PrintAll(commandLine.nextTextOut + Const.PrefConfigFile),
					["profile"] => PrintAll(Const.ProfileName),
					["log"] => PrintAll(Const.LogName),
					["captions"] => WriteCaption(),
					_ => PrintAll(commandLine.nextTextOut)
				};
				break;
			case "search":
				resultOperation = commandLine.optionsOut switch
				{
					["help"] => SearchHelp(),
					["task"] => SearchPartData(Const.TaskName, commandLine.nextTextOut),
					["profile"] => SearchPartData(Const.ProfileName, commandLine.nextTextOut),
					["numbering"] => 0, ////////////////////////////////////////////////////////////////////////
					["captions"] => WriteCaption(),
					_ => SearchPartData(commandLine.nextTextOut)
				};
				break;
			case "clear":
				resultOperation = commandLine.optionsOut switch
				{
					["help"] => ClearHelp(),
					["task"] => ClearRow(Const.TaskName, commandLine.nextTextOut),
					["task", "all"] => ClearAllFile(Const.TaskName),
					["profile"] => ClearRow(Const.ProfileName, commandLine.nextTextOut),
					["profile", "all"] => ClearAllFile(Const.ProfileName),
					["console"] => ConsoleClear(),
					["all"] => ClearAllFile(commandLine.nextTextOut),
					_ => ClearRow(commandLine.nextTextOut)
				};
				break;
			case "edit":
				resultOperation = commandLine.optionsOut switch
				{
					["help"] => EditHelp(),
					["task"] => EditRow(Const.TaskName, commandLine.nextTextOut),
					["task", "index"] => FixingIndexing(Const.TaskName),
					["task", "bool"] => EditBoolRow(Const.TaskName, commandLine.nextTextOut),
					["bool"] => EditBoolRow(commandLine.nextTextOut),
					["index"] => FixingIndexing(commandLine.nextTextOut),
					["all"] => ClearAllFile(commandLine.nextTextOut),
					_ => EditRow(commandLine.nextTextOut)
				};
				break;
			case "help":
				Help();
				break;
			case "exit":
				Environment.Exit(0);
				break;
		}

	}
}