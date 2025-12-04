namespace TodoList
{
    public class ReadCommand : ICommand
    {
        private readonly int _index;

        public ReadCommand(int index)
        {
            _index = index;
        }

        public void Execute()
        {
            if (_index < 1 || _index > AppInfo.Todos.Count)  // Изменено: убрано .Todos
            {
                Console.WriteLine("Задача с таким индексом не найдена.");
                return;
            }

            try
            {
                TodoItem item = AppInfo.Todos[_index - 1];
                Console.WriteLine(item.GetFullInfo());
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Задача с таким индексом не найдена.");
            }
        }

        public void Unexecute() { }
    }
}