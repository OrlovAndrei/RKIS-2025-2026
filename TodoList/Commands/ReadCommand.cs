using TodoList.Exceptions;

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
                throw new AuthenticationException("Необходимо войти в профиль.");

            if (_index < 1 || _index > AppInfo.CurrentTodos.Count)
                throw new TaskNotFoundException($"Задача с индексом {_index} не найдена.");

            TodoItem item = AppInfo.CurrentTodos[_index - 1];
            Console.WriteLine(item.GetFullInfo());
        }

        public void Unexecute() { }
    }
}