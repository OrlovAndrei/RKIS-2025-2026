namespace TodoList.commands
{
    public class ViewCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public bool ShowIndex { get; set; }
        public bool ShowDone { get; set; }
        public bool ShowDate { get; set; }

        public void Execute()
        {
            TodoList.View(ShowIndex, ShowDone, ShowDate);
        }
    }
}