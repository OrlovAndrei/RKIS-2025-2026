using System;
using System.Text;

class AddCommand : ICommand
{
    public TodoList TodoList { get; set; }
    public string Args { get; set; }
    public bool Multiline { get; set; }

    public AddCommand(TodoList todoList, string args)
    {
        TodoList = todoList;
        Args = args ?? string.Empty;
        Multiline = Args.Contains("--multiline", StringComparison.OrdinalIgnoreCase) ||
                    Args.Contains("-m", StringComparison.OrdinalIgnoreCase);
    }

    public void Execute()
    {
        if (Multiline)
        {
            Console.WriteLine("Многострочный режим. Введите строки задачи. Введите '!end' на отдельной строке чтобы завершить.");
            string line;
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                line = Console.ReadLine();
                if (line != null && line.Trim() == "!end")
                    break;
                if (line != null)
                {
                    if (sb.Length > 0)
                        sb.Append('\n');
                    sb.Append(line);
                }
            }

            string taskText = sb.ToString();
            TodoItem item = new TodoItem(taskText);
            TodoList.Add(item);

            Console.WriteLine("Многострочная задача добавлена.");
            return;
        }

        if (string.IsNullOrWhiteSpace(Args))
        {
            Console.WriteLine("Ошибка: укажите текст задачи. Пример: add \"Сделать задание\"");
            return;
        }

        string taskTextSingle = Args.Trim().Trim('"');
        TodoItem itemSingle = new TodoItem(taskTextSingle);
        TodoList.Add(itemSingle);

        Console.WriteLine($"Задача добавлена: \"{taskTextSingle}\"");
    }
}

