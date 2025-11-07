namespace TodoList
{
    public class ViewCommand : ICommand
    {
        public bool ShowIndex { get; set; }
        public bool ShowStatus { get; set; }
        public bool ShowDate { get; set; }
        public bool ShowAll { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            if (ShowAll)
            {
                ShowIndex = ShowStatus = ShowDate = true;
            }

            TodoList.View(ShowIndex, ShowStatus, ShowDate);
        }
    }
}