namespace ShevricTodo.Parser;

internal class RunOptions
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
					// await Commands.TaskVerb.Remove.Done(
					// 	searchTaskTodo: Commands.TaskVerb.Search.SearchTasksContains,
					// 	inputBool: Input.Button.YesOrNo,
					// 	inputOneOf: Input.OneOf.GetOneFromList,
					// 	showTaskTodo: Commands.TaskVerb.Show.ShowTask,
					// 	showMessage: Console.WriteLine,
					// 	searchTemplate: new Database.TaskTodo());
				}
				else if (t.Search)
				{
					// await Commands.TaskVerb.Search.SearchAndPrintTasks(
					// 	searchTask: Commands.TaskVerb.Search.SearchTasksContains,
					// 	searchTemplate: new Database.TaskTodo()
					// );
				}
				break;
			// case Verb.Profile p:
			// 	if (p.Add)
			// 	{
			// 		await Commands.ProfileVerb.Add.Done(
			// 			inputString: Input.Text.ShortText,
			// 			inputBool: Input.Button.YesOrNo,
			// 			inputDateTime: Input.When.Date,
			// 			inputPassword: Input.Password.CheckingThePassword
			// 		);
			// 	}
			// 	else if (p.Change)
			// 	{
			// 		await Commands.ProfileVerb.Change.ProfileChange(
			// 			searchProfile: Commands.ProfileVerb.Search.SearchProfilesContains,
			// 			inputPassword: Input.Password.GetPassword,
			// 			inputOneOf: Input.OneOf.GetOneFromList,
			// 			showMessage: Console.WriteLine,
			// 			showProfile: Commands.ProfileVerb.Show.ShowProfile,
			// 			searchTemplate: new Database.Profile()
			// 		);
			// 	}
			// 	else if (p.List)
			// 	{
			// 		await Commands.ProfileVerb.List.PrintAllProfiles();
			// 	}
			// 	else if (p.Remove)
			// 	{
			// 		await Commands.ProfileVerb.Remove.Done(
			// 			searchProfile: Commands.ProfileVerb.Search.SearchProfilesContains,
			// 			inputPassword: Input.Password.GetPassword,
			// 			inputOneOf: Input.OneOf.GetOneFromList,
			// 			showMessage: Console.WriteLine,
			// 			showProfile: Commands.ProfileVerb.Show.ShowProfile,
			// 			searchTemplate: new Database.Profile()
			// 		);
			// 	}
			// 	else if (p.Search)
			// 	{
			// 		await Commands.ProfileVerb.Search.SearchAndPrintProfiles(
			// 			searchProfile: Commands.ProfileVerb.Search.SearchProfilesContains,
			// 			searchTemplate: new Database.Profile()
			// 		);
			// 	}
				// break;
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
		}
	}
}
