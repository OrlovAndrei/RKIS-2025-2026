using Application.Dto;
using Application.Specifications.Criteria;
using Application.UseCase.TodoTaskUseCases;
using Application.UseCase.TodoTaskUseCases.Query;
using Presentation.Adapters;
using Presentation.Output.Implementation;

namespace Presentation.Parser;

internal static class RunTaskCommands
{
    private static readonly InputAdapter _input = new();

    public async static Task Run(Verb.Task t)
    {
        if (t is null) return;

        // Add task
        if (t.Add)
        {
            var name = string.IsNullOrWhiteSpace(t.Name)
                ? _input.GetShortText("Введите название задачи: ")
                : t.Name!;

            var userContext = Launch.UserContext;

            var createDto = new TodoTaskDto.TodoTaskCreateDto(
                State: null,
                Priority: null,
                UserContext: userContext,
                Name: name,
                Description: t.Description,
                Deadline: Parse.ParseDate(t.Deadline)
            );

            var repo = Launch.TodoTaskRepository;

            var useCase = new AddNewTaskUseCase(
                repository: repo,
                userContext: userContext,
                taskCreate: createDto
            );

            await Launch.CommandManager.ExecuteCommandAsync(useCase);
            return;
        }

        // List tasks
        if (t.List)
        {
            var repo = Launch.TodoTaskRepository;
            var useCase = new GetAllTasksUseCase(repository: repo);
            var tasks = await useCase.Execute();
            PrintTasks(tasks);
            return;
        }

        // Search tasks
        if (t.Search)
        {
            var searchType = t.StartWith
                ? SearchTypes.StartsWith
                : t.EndsWith
                    ? SearchTypes.EndsWith
                    : SearchTypes.Contains;
            var searchDto = new TodoTaskDto.TodoTaskSearchDto(
                Name: t.Name,
                Description: t.Description,
                SearchType: searchType
            );

            var repo = Launch.TodoTaskRepository;
            var useCase = new FindTasksUseCase(repository: repo, searchDto: searchDto);
            var res = await useCase.Execute();
            PrintTasks(res);
            return;
        }
    }

    private static void PrintTasks(IEnumerable<TodoTaskDto.TodoTaskDetailsDto> tasks)
    {
        var cols = new[] { "Id", "Name", "Deadline", "State", "Priority" };
        var rows = tasks.Select(t => new[]
        {
            t.TaskId.ToString(),
            t.Name,
            t.Deadline?.ToShortDateString() ?? string.Empty,
            t.NameState,
            t.NamePriority
        });
        WriteToConsole.PrintTable(cols, rows);
    }
}
