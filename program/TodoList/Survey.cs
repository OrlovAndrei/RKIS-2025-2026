//This file contains every command and option for program and their logic - PoneMaurice 
namespace Task;

public class Survey
{
	public static SearchCommandOnJson? commandLineGlobal;
	public void GlobalCommand(string text)
	{
		string ask = Input.String(text);
		SearchCommandOnJson commandLine = new(ask.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
		commandLineGlobal = commandLine;
		switch (commandLine.commandOut)
		{
			case "add":
				if (commandLine.optionsOut.Length > 0)
				{
					if (commandLine.SearchOption("help"))
					{
						Commands.AddHelp();
					}
					else if (commandLine.SearchOption("task"))
					{
						Commands.AddTask();
					}
					else if (commandLine.SearchOption("multi", "task"))
					{
						Commands.MultiAddTask();
					}
					else if (commandLine.SearchOption("task", "print"))
					{
						Commands.AddTaskAndPrint();
					}
					else if (commandLine.SearchOption("config"))
					{
						Commands.AddConfUserData(commandLine.nextTextOut);
					}
					else if (commandLine.SearchOption("profile"))
					{
						Commands.AddProfile();
					}
					else { Commands.AddUserData(commandLine.nextTextOut); }
				}
				else { Commands.AddUserData(commandLine.nextTextOut); }
				break;
			case "profile":
				if (commandLine.optionsOut.Length > 0)
				{
					if (commandLine.SearchOption("help"))
					{
						Commands.ProfileHelp();
					}
					else if (commandLine.SearchOption("change"))
					{
						Commands.UseActiveProfile();
					}
					else if (commandLine.SearchOption("index"))
					{
						Commands.FixingIndexing(Const.ProfileName);
					}
				}
				else {
					OpenFile file = new(Const.ProfileName);
					Commands.Print(Commands.SearchActiveProfile(), file.GetLineFilePositionRow(0)); 
					}
				break;
			case "print":
				if (commandLine.optionsOut.Length > 0)
				{
					if (commandLine.SearchOption("help"))
					{
						Commands.PrintHelp();
					}
					else if (commandLine.SearchOption("task"))
					{
						Commands.PrintAll(Const.TaskName);
					}
					else if (commandLine.SearchOption("config"))
					{
						Commands.PrintAll(commandLine.nextTextOut + Const.PrefConfigFile);
					}
					else if (commandLine.SearchOption("profile"))
					{
						Commands.PrintAll(Const.ProfileName);
					}
					else if (commandLine.SearchOption("log"))
					{
						Commands.PrintAll(Const.LogName);
					}
					else if (commandLine.SearchOption("captions"))
					{
						Commands.WriteCaption();
					}
					else { Commands.PrintAll(commandLine.nextTextOut); }
				}
				else { Commands.PrintAll(commandLine.nextTextOut); }
				break;
			case "search":
				if (commandLine.optionsOut.Length > 0)
				{
					if (commandLine.SearchOption("help"))
					{
						Commands.SearchHelp();
					}
					else if (commandLine.SearchOption("task"))
					{
						Commands.SearchPartData(Const.TaskName, commandLine.nextTextOut);
					}
					else if (commandLine.SearchOption("profile"))
					{
						Commands.SearchPartData(Const.ProfileName, commandLine.nextTextOut);
					}
					else if (commandLine.SearchOption("numbering"))
					{
						WriteToConsole.RainbowText("Эта функция ещё в разработке", ConsoleColor.Magenta); ///////////////////////////////////////////////////////////
					}
					else { Commands.SearchPartData(commandLine.nextTextOut); }
				}
				else { Commands.SearchPartData(commandLine.nextTextOut); }
				break;
			case "clear":
				if (commandLine.optionsOut.Length > 0)
				{
					if (commandLine.SearchOption("help"))
					{
						Commands.ClearHelp();
					}
					else if (commandLine.SearchOption("task"))
					{
						Commands.ClearRow(Const.TaskName, commandLine.nextTextOut);
					}
					else if (commandLine.SearchOption("task", "all"))
					{
						Commands.ClearAllFile(Const.TaskName);
					}
					else if (commandLine.SearchOption("profile"))
					{
						Commands.ClearRow(Const.ProfileName, commandLine.nextTextOut);
					}
					else if (commandLine.SearchOption("profile", "all"))
					{
						Commands.ClearAllFile(Const.ProfileName);
					}
					else if (commandLine.SearchOption("console"))
					{
						WriteToConsole.RainbowText("CCCCCCClear", ConsoleColor.Magenta);
						Console.Clear();
					}
					else if (commandLine.SearchOption("all"))
					{
						Commands.ClearAllFile(commandLine.nextTextOut);
					}
					else { Commands.ClearRow(commandLine.nextTextOut); }
				}
				else { Commands.ClearRow(commandLine.nextTextOut); }
				break;
			case "edit":
				if (commandLine.optionsOut.Length > 0)
				{
					if (commandLine.SearchOption("help"))
					{
						Commands.EditHelp();
					}
					else if (commandLine.SearchOption("task"))
					{
						Commands.EditRow(Const.TaskName, commandLine.nextTextOut);
					}
					else if (commandLine.SearchOption("task", "index"))
					{
						Commands.FixingIndexing(Const.TaskName);
					}
					else if (commandLine.SearchOption("task", "bool"))
					{
						Commands.EditBoolRow(Const.TaskName, commandLine.nextTextOut);
					}
					else if (commandLine.SearchOption("bool"))
					{
						Commands.EditBoolRow(commandLine.nextTextOut);
					}
					else if (commandLine.SearchOption("index"))
					{
						Commands.FixingIndexing(commandLine.nextTextOut);
					}
					else { Commands.EditRow(commandLine.nextTextOut); }
				}
				else { Commands.EditRow(commandLine.nextTextOut); }
				break;
			case "help":
				Commands.Help();
				break;
			case "exit":
				Environment.Exit(0);
				break;
		}

	}
}