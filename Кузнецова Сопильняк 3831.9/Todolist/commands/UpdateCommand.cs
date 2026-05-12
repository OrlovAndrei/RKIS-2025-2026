namespace TodoList
{
    public sealed class UpdateCommand : ICommand
    {
        public int TaskIndex { get; set; }
        public string NewText { get; set; }
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

            if (string.IsNullOrWhiteSpace(NewText))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return;
            }

            string oldText = item.Text;
            if (oldText == null) oldText = "";
            item.UpdateText(NewText);

            Console.WriteLine($"Задача обновлена: \"{oldText}\" -> \"{NewText}\"");
            FileManager.SaveTodos(TodoList, "data/todo.csv");
        }
    }
}