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
            if (AppInfo.CurrentTodos == null)
            {
                Console.WriteLine("Ошибка: нет активного профиля.");
                return;
            }

            if (_index < 1 || _index > AppInfo.CurrentTodos.Count)
            {
                Console.WriteLine("Задача с таким индексом не найдена.");
                return;
            }

            try
            {
                TodoItem item = AppInfo.CurrentTodos[_index - 1];
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