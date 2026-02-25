using Application.Dto;
using Application.Specifications.Criteria;
using Application.UseCase.TodoTaskUseCases.Query;
using Presentation.Output.Implementation;
using Presentation.Parser.Verb;

namespace Presentation.Parser.TaskCommands;

internal static class SearchTasksCommand
{
    public static async Task ExecuteAsync(TaskSearch t)
    {
        var searchType = t.SearchType switch
        {
            "startswith" or "StartsWith" => SearchTypes.StartsWith,
            "endswith" or "EndsWith" => SearchTypes.EndsWith,
            "equals" or "Equals" => SearchTypes.Equals,
            _ => SearchTypes.Contains
        };

        var searchDto = new TodoTaskDto.TodoTaskSearchDto(
            TaskId: t.TaskId,
            StateId: t.StateId,
            PriorityLevelFrom: t.PriorityLevelFrom,
            PriorityLevelTo: t.PriorityLevelTo,
            UserContext: Launch.UserContext,
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
