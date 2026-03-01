using TodoList.Exceptions;

namespace TodoList
{
    public class UpdateCommand : ICommand
    {
        private readonly int _index;
        private readonly string _text;
        private readonly bool _isMultiline;
        private string _oldText;

        public UpdateCommand(int index, string text, bool isMultiline)
        {
            _index = index;
            _text = text;
            _isMultiline = isMultiline;
        }

        public void Execute()
        {
            if (AppInfo.CurrentTodos == null)
                throw new AuthenticationException("Необходимо войти в профиль.");

            if (_index < 1 || _index > AppInfo.CurrentTodos.Count)
                throw new TaskNotFoundException($"Задача с индексом {_index} не найдена.");

            TodoItem item = AppInfo.CurrentTodos[_index - 1];
            _oldText = item.Text;
            string finalText = _isMultiline ? ReadMultiline() : _text.Trim('"');
            
            if (string.IsNullOrWhiteSpace(finalText))
                throw new InvalidArgumentException("Текст задачи не может быть пустым.");
            
            AppInfo.CurrentTodos.UpdateText(_index, finalText);
            AppInfo.UndoStack.Push(this);
            AppInfo.RedoStack.Clear();
            Console.WriteLine("Обновлено.");
        }

        public void Unexecute()
        {
            if (_index >= 1 && _index <= AppInfo.CurrentTodos.Count && AppInfo.CurrentTodos != null)
            {
                AppInfo.CurrentTodos.UpdateText(_index, _oldText);
                Console.WriteLine($"Текст задачи {_index} возвращен к предыдущему значению.");
            }
        }

        private static string ReadMultiline()
        {
            Console.WriteLine("Ввод построчно, !end для конца:");
            string res = "";
            while (true)
            {
                Console.Write("> ");
                string line = Console.ReadLine();
                if (line == "!end") break;
                res += line + "\n";
            }
            return res.TrimEnd('\n');
        }
    }
}