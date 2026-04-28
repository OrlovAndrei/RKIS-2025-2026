namespace TodoList
{
    public sealed class DeleteCommand : ICommand
    {
        public int TaskIndex { get; set; }
        public bool Force { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            TodoItem item = TodoList.GetItem(TaskIndex);
            if (item == null)
            {
                Console.WriteLine($"Ошибка: Задача с индексом {TaskIndex} не найдена.");
                return;
            }

            string deletedTask = item.Text;
            if (deletedTask == null) deletedTask = "";

            if (!Force)
            {
                Console.WriteLine($"Используйте --force для удаления задачи не с конца списка");
                return;
            }

            TodoList.Delete(TaskIndex);
            Console.WriteLine($"Задача \"{deletedTask}\" удалена");
            FileManager.SaveTodos(TodoList, "data/todo.csv");
        }
    }
}