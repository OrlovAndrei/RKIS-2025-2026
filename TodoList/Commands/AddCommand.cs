using System;

namespace Todolist
{
    public class AddCommand : ICommand
    {
        public bool IsMultiline { get; private set; }
        public string TaskText { get; private set; }
        private TodoItem _addedItem;
        private int _addedIndex;

        public AddCommand(string taskText, bool isMultiline = false)
        {
            TaskText = taskText;
            IsMultiline = isMultiline;
        }

        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
            {
                Console.WriteLine("Ошибка: необходимо войти в профиль");
                return;
            }

            var todoList = AppInfo.GetCurrentTodos();

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
            _addedIndex = todoList.GetCount();
            todoList.Add(_addedItem);
            Console.WriteLine($"Добавлена задача №{_addedIndex + 1}: {TaskText}");

            FileManager.SaveTodos(todoList, AppInfo.CurrentProfileId.Value);
        }

        public void Unexecute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
                return;

            var todoList = AppInfo.GetCurrentTodos();
            
            if (_addedIndex >= 0 && _addedIndex < todoList.GetCount())
            {
                todoList.Delete(_addedIndex);
                Console.WriteLine($"Отменено добавление задачи: {TaskText}");
                
                FileManager.SaveTodos(todoList, AppInfo.CurrentProfileId.Value);
            }
        }
    }
}