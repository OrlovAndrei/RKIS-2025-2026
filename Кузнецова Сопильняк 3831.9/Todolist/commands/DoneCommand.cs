namespace TodoList
{
    public sealed class DoneCommand : ICommand
    {
        public int TaskIndex { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            TodoItem item = TodoList.GetItem(TaskIndex);
            if (item == null)
            {
                Console.WriteLine($"Ошибка: Задача с индексом {TaskIndex} не найдена.");
                return;
            }

            item.MarkDone();

            Console.WriteLine($"Задача \"{item.Text}\" отмечена как выполненная");
            FileManager.SaveTodos(TodoList, "data/todo.csv");
        }
    }
}