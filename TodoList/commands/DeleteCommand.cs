namespace TodoList.commands
{
    public class DeleteCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public int Index { get; set; }
        private TodoItem? deletedItem;

        public void Execute()
        {
            deletedItem = TodoList[Index];
            TodoList.Delete(Index);
            Console.WriteLine($"Задача #{Index + 1} удалена.");
        }

        public void Unexecute()
        {
            if (deletedItem != null)
            {
                TodoList.Add(deletedItem);
                Console.WriteLine("Удаление задачи отменено");
            }
        }
    }
}
