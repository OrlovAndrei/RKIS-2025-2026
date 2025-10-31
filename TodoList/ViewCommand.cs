namespace TodoList
{
    public class ViewCommand : ICommand
    {
        public string[] Flags { get; set; } = Array.Empty<string>();
        public TodoList TodoList { get; set; } = null!;

        public void Execute()
        {
            TodoList.ViewTasks(Flags);
        }
    }
}