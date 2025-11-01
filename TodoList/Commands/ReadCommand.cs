namespace TodoList
{
    public class ReadCommand : ICommand
    {
        private readonly TodoList todoList;
        private readonly int index;

        public ReadCommand(TodoList todoList, int index)
        {
            this.todoList = todoList;
            this.index = index;
        }

        public void Execute()
        {
            TodoItem item = todoList.GetItem(index);
            if (item == null)
                System.Console.WriteLine("Задача с таким индексом не найдена.");
            else
                System.Console.WriteLine(item.GetFullInfo());
        }
    }
}