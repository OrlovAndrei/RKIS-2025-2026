using System;
using System.Collections.Generic;

namespace Todolist
{
    public class AddCommand : ICommand
    {
        public string Input { get; set; }
        public bool IsMultiline { get; set; }
        private TodoItem _addedItem;
        private int _addedIndex;

        public void Execute()
        {
            if (IsMultiline)
            {
                AddMultilineTask();
            }
            else
            {
                AddSingleLineTask();
            }

            string dataDir = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data");
            string todoFile = System.IO.Path.Combine(dataDir, "todo.csv");
            FileManager.SaveTodos(AppInfo.Todos, todoFile);
        }

        public void Unexecute()
        {
            if (_addedItem != null && _addedIndex >= 0 && _addedIndex < AppInfo.Todos.Count)
            {
                if (AppInfo.Todos.GetItem(_addedIndex) == _addedItem)
                {
                    AppInfo.Todos.Delete(_addedIndex);
                    Console.WriteLine($"Отменено добавление: {_addedItem.Text}");
                }
            }
        }

        private void AddSingleLineTask()
        {
            string[] parts = Input.Split('"');
            if (parts.Length >= 2)
            {
                string task = parts[1];
                TodoItem newItem = new TodoItem(task);
                _addedIndex = AppInfo.Todos.Count;
                AppInfo.Todos.Add(newItem);
                _addedItem = newItem;
                Console.WriteLine($"Задача добавлена: {task}");
            }
            else
            {
                Console.WriteLine("Ошибка: используйте формат add \"текст задачи\"");
            }
        }

        private void AddMultilineTask()
        {
            Console.WriteLine("Многострочный режим. Вводите задачи (для завершения введите 'end'):");

            List<string> lines = new List<string>();
            while (true)
            {
                string line = Console.ReadLine();
                if (line?.ToLower() == "end")
                    break;

                if (!string.IsNullOrEmpty(line))
                    lines.Add(line);
            }

            if (lines.Count > 0)
            {
                string task = string.Join("\n", lines);
                TodoItem newItem = new TodoItem(task);
                _addedIndex = AppInfo.Todos.Count;
                AppInfo.Todos.Add(newItem);
                _addedItem = newItem;
                Console.WriteLine($"Добавлена многострочная задача ({lines.Count} строк)");
            }
            else
            {
                Console.WriteLine("Не добавлено ни одной строки");
            }
        }
    }
}
