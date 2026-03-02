using Application.UseCase.TodoTaskUseCases.Query;
using ConsoleApp.Parser.Verb;

namespace ConsoleApp.Parser.TaskCommands;

internal static class ListTasksCommand
{
    public static async Task ExecuteAsync(TaskList t)
    {
        var useCase = new GetAllTasksUseCase(repository: Launch.TodoTaskRepository, userContext: Launch.UserContext);
        var tasks = await useCase.Execute();

        if (t.Top.HasValue)
        {
            tasks = tasks.Take(t.Top.Value);
        }

        TaskPrinter.PrintTasks(tasks);
    }
}
