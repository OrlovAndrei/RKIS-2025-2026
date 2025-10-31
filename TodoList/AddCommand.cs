namespace TodoList
{
    public class AddCommand : ICommand
    {
        public string Text { get; set; } = string.Empty;
        public bool IsMultiline { get; set; } = false;
        public string[] Flags { get; set; } = System.Array.Empty<string>();
        public TodoList TodoList { get; set; } = null!;

        public void Execute()
        {
            TodoList.AddTask(Text, Flags);
        }
    }
}
