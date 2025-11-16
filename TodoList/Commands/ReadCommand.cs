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
            if (index < 1 || index > todoList.Todos.Count)
            {
                Console.WriteLine("Задача с таким индексом не найдена.");
                return;
            }

            try
            {
                TodoItem item = todoList[index - 1];  
                Console.WriteLine(item.GetFullInfo());
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Задача с таким индексом не найдена.");
            }
        }
    }
}