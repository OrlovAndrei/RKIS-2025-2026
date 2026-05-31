using ConsoleApp.Parser.TaskCommands;
using ConsoleApp.Parser.Verb;

namespace ConsoleApp.Parser;

internal static class RunTaskCommands
{
    public async static Task RunAdd(TaskAdd t)
    {
        await AddTaskCommand.ExecuteAsync(t);
    }

    public async static Task RunRemove(TaskRemove t)
    {
        await RemoveTaskCommand.ExecuteAsync(t);
    }

    public async static Task RunEdit(TaskEdit t)
    {
        await EditTaskCommand.ExecuteAsync(t);
    }

    public async static Task RunSearch(TaskSearch t)
    {
        await SearchTasksCommand.ExecuteAsync(t);
    }

    public async static Task RunList(TaskList t)
    {
        await ListTasksCommand.ExecuteAsync(t);
    }
}
