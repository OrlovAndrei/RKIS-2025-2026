using Application.Dto;
using ConsoleApp.Output.Implementation;

namespace ConsoleApp.Parser.TaskCommands;

internal static class TaskPrinter
{
    public static void PrintTasks(IEnumerable<TodoTaskDto.TodoTaskDetailsDto> tasks)
    {
        if (!tasks.Any())
        {
            WriteToConsole.ColorMessage("Нет задач для отображения.");
            return;
        }

        var cols = new[] { "Id", "Name", "Deadline", "State", "Priority" };
        var rows = tasks.Select(t => new[]
        {
            t.TaskId.ToString(),
            t.Name,
            t.Deadline?.ToShortDateString() ?? "-",
            t.NameState,
            t.NamePriority
        });
        WriteToConsole.PrintTable(cols, rows);
    }
}
