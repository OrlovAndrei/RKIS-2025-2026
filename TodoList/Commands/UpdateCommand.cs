namespace TodoList
{
    public class UpdateCommand : ICommand
    {
        public string Arg { get; set; } = string.Empty;
        public TodoList TodoList { get; set; } = null!;

        public void Execute()
        {
            TodoList.UpdateTask(Arg);
        }
    }
}