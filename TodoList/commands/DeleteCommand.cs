namespace TodoList.commands
{
    public class DeleteCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int Index { get; set; }

        public void Execute()
        {
            TodoList.Delete(Index);
            Console.WriteLine($"Задача #{Index + 1} удалена.");
        }
    }
}
