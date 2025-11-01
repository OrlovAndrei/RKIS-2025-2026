namespace TodoList
{
    public class ViewCommand : ICommand
    {
        private readonly TodoList todoList;
        private readonly bool showIndex;
        private readonly bool showStatus;
        private readonly bool showDate;

        public ViewCommand(TodoList todoList, bool showIndex, bool showStatus, bool showDate)
        {
            this.todoList = todoList;
            this.showIndex = showIndex;
            this.showStatus = showStatus;
            this.showDate = showDate;
        }

        public void Execute()
        {
            todoList.View(showIndex, showStatus, showDate);
        }
    }
}