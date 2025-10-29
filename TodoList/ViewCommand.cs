namespace TodoList
{
    public class ViewCommand : BaseCommand
    {
        public bool ShowIndex { get; set; }
        public bool ShowStatus { get; set; }
        public bool ShowDate { get; set; }

        public ViewCommand(TodoList todoList, bool showIndex, bool showStatus, bool showDate) : base(todoList)
        {
            ShowIndex = showIndex;
            ShowStatus = showStatus;
            ShowDate = showDate;
        }

        public override void Execute()
        {
            TodoList.View(ShowIndex, ShowStatus, ShowDate);
        }
    }
}