namespace TodoList.commands
{
    public class StatusCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public TodoStatus Status { get; set; }
        public int Index { get; set; }
        
        private TodoStatus oldStatus;

        public void Execute()
        {
            oldStatus = TodoList[Index].Status;
            TodoList[Index].SetStatus(Status);
            Console.WriteLine($"Задача #{Index + 1} изменила статус.");
        }

        public void Unexecute()
        {
            if (Index >= 0 && Index < TodoList.Count)
            {
                TodoList[Index].SetStatus(oldStatus);
                Console.WriteLine("Изменение статуса отменено");
            }
        }
    }
}
