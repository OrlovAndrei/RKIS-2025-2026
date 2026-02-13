namespace TodoList
{
    public sealed class ViewCommand : ICommand
    {
        public bool ShowIndex { get; set; }
        public bool ShowStatus { get; set; }
        public bool ShowDate { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            TodoList.View(ShowIndex, ShowStatus, ShowDate);
        }
    }
}