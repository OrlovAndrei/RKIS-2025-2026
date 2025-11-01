namespace TodoList
{
    public class DoneCommand : ICommand
    {
        private TodoList TodoList { get; }
        public int Index { get; set; }

        public DoneCommand(TodoList todoList, int index)
        {
            TodoList = todoList;
            Index = index;
        }

        public void Execute()
        {
            TodoItem item = TodoList.GetItem(Index);
            if (item == null)
                System.Console.WriteLine("Задача с таким индексом не найдена.");
            else
            {
                item.MarkDone();
                System.Console.WriteLine("Готово.");
            }
        }
    }
}