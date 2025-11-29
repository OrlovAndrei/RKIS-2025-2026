using System;
using System.Text;

namespace Todolist.Commands
{
    internal class AddCommand : ICommand
    {
        public string Args { get; set; }
        public bool Multiline { get; set; }
        private int? addedIndex = null;
        private string? taskText = null;

        public AddCommand(string args)
        {
            Args = args ?? string.Empty;
            Multiline = Args.Contains("--multiline", StringComparison.OrdinalIgnoreCase) ||
                        Args.Contains("-m", StringComparison.OrdinalIgnoreCase);
        }

        public void Execute()
        {
            if (Multiline)
            {
                Console.WriteLine("Многострочный режим. Введите строки задачи. Введите '!end' на отдельной строке чтобы завершить.");
                StringBuilder sb = new StringBuilder();
                while (true)
                {
                    string? line = Console.ReadLine();
                    if (line != null && line.Trim() == "!end")
                        break;
                    if (line != null)
                    {
                        if (sb.Length > 0)
                            sb.Append('\n');
                        sb.Append(line);
                    }
                }

                taskText = sb.ToString();
                TodoItem item = new TodoItem(taskText);
                addedIndex = AppInfo.Todos.Count + 1;
                AppInfo.Todos.Add(item);
                FileManager.SaveTodos(AppInfo.Todos, Program.TodoFilePath);

                Console.WriteLine("Многострочная задача добавлена.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Args))
            {
                Console.WriteLine("Ошибка: укажите текст задачи. Пример: add \"Сделать задание\"");
                return;
            }

            taskText = Args.Trim().Trim('"');
            TodoItem itemSingle = new TodoItem(taskText);
            addedIndex = AppInfo.Todos.Count + 1;
            AppInfo.Todos.Add(itemSingle);
            FileManager.SaveTodos(AppInfo.Todos, Program.TodoFilePath);

            Console.WriteLine($"Задача добавлена: \"{taskText}\"");
        }

        public void Unexecute()
        {
            if (addedIndex.HasValue && addedIndex.Value > 0 && addedIndex.Value <= AppInfo.Todos.Count)
            {
                AppInfo.Todos.Delete(addedIndex.Value);
                FileManager.SaveTodos(AppInfo.Todos, Program.TodoFilePath);
            }
        }
    }
}
