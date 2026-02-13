namespace TodoList.Commands;

public class AddCommand : ICommand
{
    public string input { get; set; }
    public TodoList todos { get; set; }

    public void Execute()
    {
        var flags = CommandParser.ParseFlags(input);
        var isMultiTask = flags.Contains("-m") || flags.Contains("--multi");

        if (isMultiTask)
        {
            var taskText = "";
            Console.WriteLine("Многострочный режим, введите !end для отправки");

            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (line == "!end") break;
                taskText += line + "\n";
            }

            AddTask(taskText);
        }
        else
        {
            var taskText = ExtractTaskText(input);
            AddTask(taskText);
        }
    }

    private void AddTask(string taskText)
    {
        if (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            Console.WriteLine("Формат: add \"текст задачи\"");
            return;
        }

        todos.Add(new TodoItem(taskText));
    }

    private string ExtractTaskText(string input)
    {
        var parts = input.Split('"');

        if (parts.Length >= 2)
            return parts[1];
        return input.Substring(3).Trim();
    }
}