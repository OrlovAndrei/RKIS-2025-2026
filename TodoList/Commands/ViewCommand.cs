namespace TodoList.Commands
{
    public class ViewCommand : ICommand
    {
        public bool ShowAll { get; set; } = false;
        public bool ShowIndex { get; set; } = false;
        public bool ShowStatus { get; set; } = false;
        public bool ShowDate { get; set; } = false;
        public string[] Flags { get; set; } = System.Array.Empty<string>();
        public TodoList TodoList { get; set; } = null!;

        public void Execute()
        {
            TodoList.ViewTasks(Flags);
        }
    }
}
