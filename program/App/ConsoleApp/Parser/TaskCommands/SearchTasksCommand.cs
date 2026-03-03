using Application.Dto;
using Application.Specifications.Criteria;
using Application.UseCase.TodoTaskUseCases.Query;
using ConsoleApp.Output.Implementation;
using ConsoleApp.Parser.Verb;

namespace ConsoleApp.Parser.TaskCommands;

internal static class SearchTasksCommand
{
    public static async Task ExecuteAsync(TaskSearch t)
    {
        var searchType = t.SearchType?.ToLower() switch
        {
            "startswith" => SearchTypes.StartsWith,
            "endswith" => SearchTypes.EndsWith,
            "equals" => SearchTypes.Equals,
            _ => SearchTypes.Contains
        };

        var searchDto = new TodoTaskDto.TodoTaskSearchDto(
			UserContext: Launch.UserContext,
			TaskId: t.TaskId,
			StateId: t.StateId,
			PriorityLevelFrom: t.PriorityLevelFrom,
			PriorityLevelTo: t.PriorityLevelTo,
			Name: t.Name,
			Description: t.Description,
			CreatedAtFrom: t.CreatedAtFrom,
			CreatedAtTo: t.CreatedAtTo,
			DeadlineFrom: t.DeadlineFrom,
			DeadlineTo: t.DeadlineTo,
			SearchType: searchType
		);

        var useCase = new FindTasksUseCase(repository: Launch.TodoTaskRepository, searchDto: searchDto);
        var res = await useCase.Execute();

        if (!res.Any())
        {
            WriteToConsole.ColorMessage("Задачи не найдены.");
            return;
        }

        TaskPrinter.PrintTasks(res);
    }
}
