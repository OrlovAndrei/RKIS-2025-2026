using System;
using System.Text;
using Todolist.Exceptions;

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
            if (AppInfo.CurrentProfileId == Guid.Empty)
                throw new AuthenticationException("Необходимо войти в профиль для работы с задачами.");
            if (Multiline)
            {
                Console.WriteLine("Многострочный ввод. Введите строки, завершите '!end'.");
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
                if (string.IsNullOrWhiteSpace(taskText))
                    throw new InvalidArgumentException("Текст задачи не может быть пустым.");
                TodoItem item = new TodoItem(taskText);
                addedIndex = AppInfo.Todos.Count + 1;
                AppInfo.Todos.Add(item);

                Console.WriteLine("Задача добавлена.");
                return;
            }

            if (string.IsNullOrWhiteSpace(Args))
                throw new InvalidArgumentException("Не указан текст задачи. Пример: add \"Купить молоко\"");

            taskText = Args.Trim().Trim('"');
            TodoItem itemSingle = new TodoItem(taskText);
            addedIndex = AppInfo.Todos.Count + 1;
            AppInfo.Todos.Add(itemSingle);

            Console.WriteLine($"Добавлена задача: \"{taskText}\"");
        }

        public void Unexecute()
        {
            if (addedIndex.HasValue && addedIndex.Value > 0 && addedIndex.Value <= AppInfo.Todos.Count)
            {
                AppInfo.Todos.Delete(addedIndex.Value);
            }
        }
    }
}

