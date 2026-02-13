namespace ShevricTodo.Parser;

internal static class RunOptions
{
	public async static void Run(object obj)
	{
		switch (obj)
		{
			case Verb.Task t:
				if (t.Add)
				{
					await Commands.TaskObj.Add.Done(
						searchTemplate: new Database.TaskTodo());
				}
				else if (t.List)
				{
					await Commands.TaskObj.List.PrintAllTasksOfActiveUser();
				}
				else if (t.Remove)
				{
					await Commands.TaskObj.Remove.DoneStartsWith(
						searchTemplate: new Database.TaskTodo()
					);
				}
				else if (t.Search)
				{
					await Commands.TaskObj.Search.SearchContainsAndPrintTasksOfActiveUser(
						searchTemplate: new Database.TaskTodo()
					);
				}
				break;
			case Verb.Profile p:
				if (p.Add)
				{
					await Commands.ProfileObj.Add.Done(
						newProfile: new Database.Profile());
				}
				else if (p.Change)
				{
					await Commands.ProfileObj.Change.ProfileStartsWithChange(
						searchTemplate: new Database.Profile()
					);
				}
				else if (p.List)
				{
					await Commands.ProfileObj.List.PrintAllProfiles();
				}
				else if (p.Remove)
				{
					await Commands.ProfileObj.Remove.DoneStartsWith(
						searchTemplate: new Database.Profile()
					);
				}
				else if (p.Search)
				{
					await Commands.ProfileObj.Search.SearchStartsWithAndPrintProfiles(
						searchTemplate: new Database.Profile()
					);
				}
				break;
			case Verb.Redo r:
				//
				break;
			case Verb.Undo u:
				//
				break;
			case Verb.Exit e:
				await Commands.Exit.Done();
				break;
			case Verb.Run e:
				await Program.Run();
				break;
			default:
				return;
		}
	}
}
