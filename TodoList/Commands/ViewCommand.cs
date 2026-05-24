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
            
            bool showIndex = ShowIndex || ShowAll;
            bool showStatus = ShowStatus || ShowAll;
            bool showDate = ShowDate || ShowAll;

            todoList.View(showIndex, showStatus, showDate);
        }

        public void Unexecute()
        {
            throw new NotImplementedException("Команда view не поддерживает отмену");
        }
    }
}