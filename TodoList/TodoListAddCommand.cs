using System;
using System.Collections.Generic;

namespace TodoList
{
    /// <summary>
    /// Команда добавления новой задачи.
    /// </summary>
    internal class AddCommand : ICommand
    {
        public TodoList? TodoList { get; set; }
        public string? TaskText { get; set; }
        public bool Multiline { get; set; }
        public string? TodoFilePath { get; set; }

        public void Execute()
        {
            if (TodoList == null)
            {
                Console.WriteLine("Ошибка: список задач не установлен.");
                return;
            }

            string? text = TaskText;

            if (Multiline)
            {
                Console.WriteLine("Введите текст задачи (для завершения введите пустую строку):");
                var lines = new List<string>();
                while (true)
                {
                    string? line = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        break;
                    lines.Add(line);
                }
                text = string.Join("\n", lines);
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                Console.WriteLine("Некорректный формат. Используйте: add \"текст задачи\" или add --multiline");
                return;
            }

            var item = new TodoItem(text.Trim());
            TodoList.Add(item);

            if (!string.IsNullOrWhiteSpace(TodoFilePath))
            {
                FileManager.SaveTodos(TodoList, TodoFilePath);
            }

            Console.WriteLine($"Добавлена задача: \"{text.Trim()}\"");
        }
    }
}

