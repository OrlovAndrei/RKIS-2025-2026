using System;

namespace Todolist
{
    public class AddCommand : ICommand
    {
        public bool IsMultiline { get; private set; }
        public string TaskText { get; private set; }
        public Todolist TodoList { get; private set; }
        private TodoItem _addedItem;
        private int _addedIndex;

        public AddCommand(Todolist todoList, string taskText, bool isMultiline = false)
        {
            TodoList = todoList;
            TaskText = taskText;
            IsMultiline = isMultiline;
        }

        public void Execute()
        {
            if (IsMultiline)
            {
                Console.WriteLine("Многострочный режим. Введите задачи (для завершения введите '!end'):");
                string multilineText = "";

                Console.Write("> ");
                string line = Console.ReadLine();
                while (line.ToLower() != "!end")
                {
                    Console.Write("> ");
                    multilineText += line + "\n";
                    line = Console.ReadLine();
                }

                TaskText = multilineText.Trim();
            }
            else if (TaskText.StartsWith("\"") && TaskText.EndsWith("\""))
            {
                TaskText = TaskText.Substring(1, TaskText.Length - 2);
            }

            _addedItem = new TodoItem(TaskText);
            _addedIndex = TodoList.GetCount();
            TodoList.Add(_addedItem);
            Console.WriteLine($"Добавлена задача №{_addedIndex + 1}: {TaskText}");
        }

        public void Unexecute()
        {
            if (_addedIndex >= 0 && _addedIndex < TodoList.GetCount())
            {
                TodoList.Delete(_addedIndex);
                Console.WriteLine($"Отменено добавление задачи: {TaskText}");
            }
        }
    }
}