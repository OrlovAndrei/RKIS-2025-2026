namespace TodoList.commands
{
    public class StatusCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public TodoStatus Status { get; set; }
        public int Index { get; set; }

        public void Execute()
        {
            TodoList[Index].SetStatus(Status);
            Console.WriteLine($"Задача #{Index + 1} изменила статус.");
        }
    }
}
