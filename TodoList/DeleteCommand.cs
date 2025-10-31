namespace TodoList
{
    public class DeleteCommand : ICommand
    {
        public string Arg { get; set; } = string.Empty;
        public TodoList TodoList { get; set; } = null!;

        public void Execute()
        {
            TodoList.DeleteTask(Arg);
        }
    }
}