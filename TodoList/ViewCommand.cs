using System;

namespace TodoList
{
    public class ViewCommand : ICommand
    {
        private readonly TodoList _todoList;
        public bool ShowIndex { get; set; }
        public bool ShowStatus { get; set; }
        public bool ShowDate { get; set; }
        public bool ShowAll { get; set; }

        public ViewCommand(TodoList todoList)
        {
            _todoList = todoList;
        }

        public void Execute()
        {
            bool showIndex = ShowIndex || ShowAll;
            bool showStatus = ShowStatus || ShowAll;
            bool showDate = ShowDate || ShowAll;

            _todoList.View(showIndex, showStatus, showDate);
        }
    }
}