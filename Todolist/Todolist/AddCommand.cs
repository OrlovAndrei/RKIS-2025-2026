namespace TodoList;

public class AddCommand
{
    public string input { get; set; }
    public TodoList todos { get; set; }

    public void Execute()
    {
        var flags = CommandParser.ParseFlags(input);
        bool isMultiTask = flags.Contains("-m") || flags.Contains("--multi");
            
        if (isMultiTask)
        {
            string taskText = "";
            Console.WriteLine("Многострочный режим, введите !end для отправки");

            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line == "!end") break;
                taskText += line + "\n";
            }

            AddTask(taskText);
        }
        else
        {
            string taskText = ExtractTaskText(input);
            AddTask(taskText);
        }
    }
    void AddTask(string taskText)
    {
        if (string.IsNullOrWhiteSpace(taskText))
        {
            Console.WriteLine("Ошибка: текст задачи не может быть пустым");
            Console.WriteLine("Формат: add \"текст задачи\"");
            return;
        }

        todos.Add(new TodoItem(taskText));
    }

    string ExtractTaskText(string input)
    {
        string[] parts = input.Split('"');
            
        if (parts.Length >= 2)
        {
            return parts[1];
        }
        else
        {
            return input.Substring(3).Trim();
        }
    }
}