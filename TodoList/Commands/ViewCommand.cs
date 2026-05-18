using System;

namespace TodoList
{
    public class ViewCommand : ICommand
    {
        public bool ShowIndex { get; private set; }
        public bool ShowDone { get; private set; }
        public bool ShowDate { get; private set; }
        public TodoList TodoList { get; private set; }

        public ViewCommand(TodoList todoList, bool showIndex = false, bool showDone = false, bool showDate = false)
        {
            TodoList = todoList;
            ShowIndex = showIndex;
            ShowDone = showDone;
            ShowDate = showDate;
        }

        public void Execute()
        {
            TodoList.View(ShowIndex, ShowDone, ShowDate);
        }
    }
}