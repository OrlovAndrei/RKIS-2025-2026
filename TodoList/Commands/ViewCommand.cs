using System;

namespace Todolist
{
    public class ViewCommand : ICommand
    {
        public bool ShowIndex { get; private set; }
        public bool ShowStatus { get; private set; }
        public bool ShowDate { get; private set; }
        public bool ShowAll { get; private set; }

        public ViewCommand(bool showIndex = false, bool showStatus = false, bool showDate = false, bool showAll = false)
        {
            ShowIndex = showIndex;
            ShowStatus = showStatus;
            ShowDate = showDate;
            ShowAll = showAll;
        }

        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
            {
                Console.WriteLine("Ошибка: необходимо войти в профиль");
                return;
            }

            var todoList = AppInfo.GetCurrentTodos();
            
            if (todoList.GetCount() == 0)
            {
                Console.WriteLine("Задачи отсутствуют");
                return;
            }

            bool showIndex = ShowIndex || ShowAll;
            bool showStatus = ShowStatus || ShowAll;
            bool showDate = ShowDate || ShowAll;

            Console.WriteLine("=== Список задач ===");
            
            string header = "";
            if (showIndex) header += "№\t";
            header += "Задача";
            if (showDate) header += "\t\tДата изменения";
            if (showStatus) header += "\tСтатус";

            Console.WriteLine(header);
            Console.WriteLine(new string('-', Math.Max(header.Length, 50)));

            for (int i = 0; i < todoList.GetCount(); i++)
            {
                TodoItem item = todoList.GetItem(i);
                string row = "";

                if (showIndex) 
                    row += $"{i + 1}\t";

                string taskText = item.GetShortInfo();
                row += taskText;

                if (showDate)
                    row += $"\t{item.LastUpdate:dd.MM.yyyy HH:mm}";

                if (showStatus)
                    row += $"\t{item.Status}";

                Console.WriteLine(row);
            }

            Console.WriteLine($"Всего задач: {todoList.GetCount()}");
            Console.WriteLine("====================");
        }

        public void Unexecute()
        {
            throw new NotImplementedException("Команда view не поддерживает отмену");
        }
    }
}