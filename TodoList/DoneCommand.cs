namespace TodoList
{
    public class DoneCommand : ICommand
    {
        public string Arg { get; set; } = string.Empty;
        public TodoList TodoList { get; set; } = null!;

        public void Execute()
        {
            TodoList.MarkTaskDone(Arg);
        }
    }
}