namespace TodoList.commands
{
    public class ReadCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int Index { get; set; }

        public void Execute()
        {
            Console.WriteLine(TodoList[Index].GetFullInfo());
        }
    }
}
