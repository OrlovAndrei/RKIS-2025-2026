namespace TodoList
{
    public class AddCommand : ICommand
    {
        public string[] Flags { get; set; } = Array.Empty<string>();
        public string Text { get; set; } = string.Empty;
        public TodoList TodoList { get; set; } = null!;

        public void Execute()
        {
            TodoList.AddTask(Text, Flags);
        }
    }
}