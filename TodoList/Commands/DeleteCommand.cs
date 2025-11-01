namespace TodoList
{
    public class DeleteCommand : ICommand
    {
        private TodoList TodoList { get; }
        public int Index { get; set; }

        public DeleteCommand(TodoList todoList, int index)
        {
            TodoList = todoList;
            Index = index;
        }

        public void Execute()
        {
            if (!TodoList.Delete(Index))
                System.Console.WriteLine("Задача с таким индексом не найдена.");
            else
                System.Console.WriteLine("Удалено.");
        }
    }
}