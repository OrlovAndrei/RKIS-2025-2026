using Application.UseCase.TodoTaskUseCases.Query;
using Presentation.Parser.Verb;

namespace Presentation.Parser.TaskCommands;

internal static class ListTasksCommand
{
    public static async Task ExecuteAsync(TaskList t)
    {
        var useCase = new GetAllTasksUseCase(repository: Launch.TodoTaskRepository);
        var tasks = await useCase.Execute();

        if (t.Top.HasValue)
        {
            tasks = tasks.Take(t.Top.Value);
        }

        TaskPrinter.PrintTasks(tasks);
    }
}
