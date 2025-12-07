namespace TodoList.commands
{
    public class DoneCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int Index { get; set; }

        public void Execute()
        {
            TodoList[Index].MarkDone();
            Console.WriteLine($"Задача #{Index + 1} отмечена выполненной.");
        }
    }
}
