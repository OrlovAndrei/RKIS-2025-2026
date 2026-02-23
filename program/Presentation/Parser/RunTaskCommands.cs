using Application.Dto;
using Application.Specifications;
using Application.UseCase.TodoTaskUseCases;
using Presentation.Input;

namespace Presentation.Parser;

internal static class RunTaskCommands
{
    public async static Task Run(Verb.Task t)
    {
        if (t is null) return;

        // Add task
        if (t.Add)
        {
            var name = string.IsNullOrWhiteSpace(t.Name)
                ? Text.ShortText("Введите название задачи: ")
                : t.Name!;

            var userContext = Launch.UserContext ?? throw new Exception("User context not initialized. Call Launch.UpdateRepositories first.");

            var createDto = new TodoTaskDto.TodoTaskCreateDto(
                State: null,
                Priority: null,
                UserContext: userContext,
                Name: name,
                Description: t.Description,
                Deadline: Parse.ParseDate(t.Deadline)
            );

            var repo = Launch.TodoTaskRepository ?? throw new Exception("TodoTask repository not initialized. Call Launch.UpdateRepositories first.");

            var useCase = new AddNewTaskUseCase(
                repository: repo,
                userContext: userContext,
                taskCreate: createDto
            );

            await useCase.Execute();
            return;
        }

        // List tasks
        if (t.List)
        {
            var repo = Launch.TodoTaskRepository ?? throw new Exception("TodoTask repository not initialized.");
            var useCase = new GetAllTasksUseCase(repository: repo);
            var tasks = await useCase.Execute();
            PrintTasks(tasks);
            return;
        }

        // Search tasks
        if (t.Search)
        {
            var searchType = t.StartWith
                ? SearchType.StartsWith
                : t.EndsWith
                    ? SearchType.EndsWith
                    : SearchType.Contains;
            var searchDto = new TodoTaskDto.TodoTaskSearchDto(
                Name: t.Name,
                Description: t.Description,
                SearchType: searchType
            );

            var repo = Launch.TodoTaskRepository ?? throw new Exception("TodoTask repository not initialized.");
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
