namespace TodoList
{
    public sealed class ReadCommand : ICommand
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

            Console.WriteLine(item.GetFullInfo());
        }
    }
}