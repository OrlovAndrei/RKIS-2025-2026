using System;
using System.Collections.Generic;

namespace TodoList
{
    /// <summary>
    /// Команда добавления новой задачи.
    /// </summary>
    internal class AddCommand : ICommand
    {
        public string? TaskText { get; set; }
        public bool Multiline { get; set; }

        private int _createdIndex = -1;

        public void Execute()
        {
            if (AppInfo.CurrentProfileId == null)
            {
                Console.WriteLine("Ошибка: необходимо войти в профиль.");
                return;
            }

            if (AppInfo.Todos == null)
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
            AppInfo.Todos.Add(item);
            _createdIndex = AppInfo.Todos.Count - 1;

            Console.WriteLine($"Добавлена задача: \"{text.Trim()}\"");
        }

        public void Unexecute()
        {
            if (AppInfo.CurrentProfileId == null || AppInfo.Todos == null)
                return;

            if (_createdIndex >= 0 && _createdIndex < AppInfo.Todos.Count)
            {
                AppInfo.Todos.Delete(_createdIndex);
            }
        }
    }
}

