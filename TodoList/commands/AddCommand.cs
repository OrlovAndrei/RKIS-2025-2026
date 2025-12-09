using System;
using System.Collections.Generic;
using TodoList.commands;

namespace TodoList
{
    public class AddCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public string Text { get; set; }
        public bool IsMultiline { get; set; }
        
        
        public TodoItem AddedItem { get; set; }
        public int AddedIndex { get; set; }

        public void Execute()
        {
            if (IsMultiline)
            {
                Console.WriteLine("Введите строки задачи (каждая с префиксом '>'). Для завершения введите '!end':");
                List<string> lines = new List<string>();

                while (true)
                {
                    Console.Write("> ");
                    string line = Console.ReadLine();
                    if (line == "!end")
                        break;

                    if (!string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                }

                if (lines.Count == 0)
                {
                    Console.WriteLine("Задача не добавлена: текст пуст.");
                    return;
                }

                Text = string.Join("\n", lines);
            }

            if (string.IsNullOrWhiteSpace(Text))
            {
                Console.WriteLine("Ошибка: текст задачи не может быть пустым.");
                return;
            }

            var item = new TodoItem(Text);
            AddedItem = item;
            AddedIndex = TodoList.Count;
            TodoList.Add(item);
            Console.WriteLine(IsMultiline
                ? "Добавлена многострочная задача."
                : $"Добавлена задача: \"{Text}\"");
        }

        public void Unexecute()
        {
            if (AddedIndex < TodoList.Count && TodoList[AddedIndex] == AddedItem)
            {
                TodoList.Delete(AddedIndex);
                Console.WriteLine("Добавление задачи отменено");
            }
        }
    }
}
