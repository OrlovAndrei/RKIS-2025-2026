using System;

namespace TodoApp.Commands
{
    public class AddCommand : BaseCommand
    {
        public override string Name => "add";
        public override string Description => "Добавить новую задачу";

        // Флаги команд как свойства bool
        public bool Multiline { get; set; }

        // Введённый текст
        public string TaskText { get; set; }
        
        // Для отмены нам нужно знать, какая задача была добавлена
        private TodoItem addedItem;
        private int addedIndex = -1;

        public override bool Execute()
        {
            if (AppInfo.Todos == null)
            {
                Console.WriteLine(" Ошибка: TodoList не установлен");
                return false;
            }

            bool success;
            if (Multiline)
            {
                success = AddMultilineTask();
            }
            else
            {
                success = AddSingleLineTask();
            }

            // Сохраняем команду в стек undo
            if (success)
            {
                PushToUndoStack();
                AutoSave();
            }

            return success;
        }

        public override bool Unexecute()
        {
            if (addedIndex >= 0 && AppInfo.Todos != null)
            {
                try
                {
                    // Удаляем добавленную задачу
                    AppInfo.Todos.Delete(addedIndex);
                    Console.WriteLine($" Отмена: удалена задача '{GetShortText(addedItem.Text)}'");
                    AutoSave();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($" Ошибка при отмене добавления: {ex.Message}");
                    return false;
                }
            }
            return false;
        }

        private bool AddSingleLineTask()
        {
            string taskText = TaskText;

            if (string.IsNullOrWhiteSpace(taskText))
            {
                Console.Write("Введите текст задачи: ");
                taskText = Console.ReadLine()?.Trim();
            }

            if (!string.IsNullOrWhiteSpace(taskText))
            {
                taskText = taskText.StartsWith("\"") && taskText.EndsWith("\"") 
                    ? taskText.Substring(1, taskText.Length - 2) 
                    : taskText;

                addedItem = new TodoItem(taskText);
                AppInfo.Todos.Add(addedItem);
                addedIndex = AppInfo.Todos.Count - 1;
                Console.WriteLine(" Задача успешно добавлена!");
                return true;
            }
            else
            {
                Console.WriteLine(" Ошибка: текст задачи не может быть пустым!");
                return false;
            }
        }

        private bool AddMultilineTask()
        {
            Console.WriteLine("Введите текст задачи (для завершения введите !end):");
            System.Text.StringBuilder taskText = new System.Text.StringBuilder();
            string line;

            while (true)
            {
                Console.Write("> ");
                line = Console.ReadLine()?.Trim() ?? "";
                
                if (line == "!end")
                    break;
                    
                if (taskText.Length > 0)
                    taskText.AppendLine();
                    
                taskText.Append(line);
            }

            string finalText = taskText.ToString();
            if (!string.IsNullOrWhiteSpace(finalText))
            {
                addedItem = new TodoItem(finalText);
                AppInfo.Todos.Add(addedItem);
                addedIndex = AppInfo.Todos.Count - 1;
                Console.WriteLine(" Задача успешно добавлена!");
                return true;
            }
            else
            {
                Console.WriteLine(" Ошибка: текст задачи не может быть пустым!");
                return false;
            }
        }

        private string GetShortText(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            string shortText = text.Replace("\n", " ").Replace("\r", " ");
            return shortText.Length > 30 ? shortText.Substring(0, 30) + "..." : shortText;
        }
    }
}
