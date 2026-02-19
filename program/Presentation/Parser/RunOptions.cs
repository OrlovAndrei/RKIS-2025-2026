using Application.UseCase.TodoTaskUseCases;
using Application.UseCase.ProfileUseCases;

namespace Presentation.Parser;

internal static class RunOptions
{
	public async static void Run(object obj)
	{
		try
		{
			switch (obj)
			{
				case Verb.Task t:
					if (t.Add)
					{
						// await Commands.TaskObj.Add.Done(
						// 	searchTemplate: new Database.TaskTodo()
						// 	{
						// 		Name = t.Name,
						// 		Description = t.Description,
						// 		Deadline = Parse.ParseDate(t.Deadline)
						// 	}
						// 	);
					}
					else if (t.List)
					{
						// await Commands.TaskObj.List.PrintAllTasksOfActiveUser();
					}
					else if (t.Remove)
					{
						// await Commands.TaskObj.Remove.DoneStartsWith(
						// 	searchTemplate: new Database.TaskTodo()
						// );
					}
					else if (t.Search)
					{
						if (t.StartWith)
						{
							// await Commands.TaskObj.Search.SearchStartsWithAndPrintTasksOfActiveUser(
							// searchTemplate: new Database.TaskTodo()
							// {
							// 	Name = t.Name,
							// 	Description = t.Description,
							// 	Deadline = Parse.ParseDate(t.Deadline)
							// }
							// );
						}
						else if (t.EndsWith)
						{
							// await Commands.TaskObj.Search.SearchEndsWithAndPrintTasksOfActiveUser(
							// searchTemplate: new Database.TaskTodo()
							// {
							// 	Name = t.Name,
							// 	Description = t.Description,
							// 	Deadline = Parse.ParseDate(t.Deadline)
							// }
							// );
						}
						else
						{
							// 	await Commands.TaskObj.Search.SearchContainsAndPrintTasksOfActiveUser(
							// 	searchTemplate: new Database.TaskTodo()
							// 	{
							// 		Name = t.Name,
							// 		Description = t.Description,
							// 		Deadline = Parse.ParseDate(t.Deadline)
							// 	}
							// );
						}
					}
					break;
				case Verb.Profile p:
					if (p.Add)
					{
						Program.profileUseCase.Add(new(
							FirstName: "2121",
							LastName: "dsfsd",
							DateOfBirth: new DateTime(year: 2000, day: 20, month: 2),
							PasswordHash: "0987654321"
						));
					}
					// else if (p.Change)
					// {
					// 	await Commands.ProfileObj.Change.ProfileStartsWithChange(
					// 		searchTemplate: new Database.Profile()
					// 	);
					// }
					// else if (p.List)
					// {
					// 	await Commands.ProfileObj.List.PrintAllProfiles();
					// }
					// else if (p.Remove)
					// {
					// 	await Commands.ProfileObj.Remove.DoneStartsWith(
					// 		searchTemplate: new Database.Profile()
					// 	);
					// }
					// else if (p.Search)
					// {
					// 	if (p.StartWith)
					// 	{
					// 		await Commands.ProfileObj.Search.SearchStartsWithAndPrintProfiles(
					// 		searchTemplate: new Database.Profile()
					// 		{
					// 			FirstName = p.FirstName,
					// 			LastName = p.LastName,
					// 			UserName = p.UserName,
					// 			Birthday = Parse.ParseDate(p.Birthday)
					// 		}
					// 	);
					// 	}
					// 	else if (p.EndsWith)
					// 	{
					// 		await Commands.ProfileObj.Search.SearchEndsWithAndPrintProfiles(
					// 		searchTemplate: new Database.Profile()
					// 		{
					// 			FirstName = p.FirstName,
					// 			LastName = p.LastName,
					// 			UserName = p.UserName,
					// 			Birthday = Parse.ParseDate(p.Birthday)
					// 		}
					// 	);
					// 	}
					// 	else
					// 	{
					// 		await Commands.ProfileObj.Search.SearchContainsAndPrintProfiles(
					// 		searchTemplate: new Database.Profile()
					// 		{
					// 			FirstName = p.FirstName,
					// 			LastName = p.LastName,
					// 			UserName = p.UserName,
					// 			Birthday = Parse.ParseDate(p.Birthday)
					// 		}
					// 	);
					// 	}
					// }
					break;
				case Verb.Redo r:
					//
					break;
				case Verb.Undo u:
					//
					break;
				case Verb.Exit e:
					Program.Exit();
					break;
				case Verb.Run e:
					await Program.Run();
					break;
				default:
					throw new Exception("Hui");
			}
		}
		catch (Exception ex)
		{
			Input.WriteToConsole.ProcExcept(ex);
		}
	}
}
