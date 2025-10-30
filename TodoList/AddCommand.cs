using System;
using System.Collections.Generic;

namespace TodoList
{
    public class AddCommand : ICommand
    {
        public bool IsMultiline { get; set; }
        public string TaskText { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            if (IsMultiline)
            {
                AddMultilineTask();
            }
            else
            {
                AddSingleTask();
            }
        }

        private void AddMultilineTask()
        {
            Console.WriteLine("Многострочный режим. Введите задачи (для завершения введите '!end'):");
            List<string> lines = new List<string>();

            string line;
            while ((line = Console.ReadLine()) != null && line.ToLower() != "!end")
            {
                lines.Add(line);
            }

            if (lines.Count == 0)
            {
                Console.WriteLine("Не было введено ни одной строки");
                return;
            }

            string multilineText = string.Join("\n", lines);
            var item = new TodoItem(multilineText);
            TodoList.Add(item);
            Console.WriteLine($"Добавлена многострочная задача №{TodoList.Count}:");
            Console.WriteLine(multilineText);
        }

        private void AddSingleTask()
        {
            string taskText = TaskText;
            if (taskText.StartsWith("\"") && taskText.EndsWith("\""))
            {
                taskText = taskText.Substring(1, taskText.Length - 2);
            }

            var item = new TodoItem(taskText);
            TodoList.Add(item);
            Console.WriteLine($"Добавлена задача №{TodoList.Count}: {taskText}");
        }
    }
}