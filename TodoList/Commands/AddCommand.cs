using TodoList.Exceptions;

namespace TodoList
{
    public class AddCommand : ICommand
    {
        private readonly string _text;
        private readonly bool _isMultiline;
        private TodoItem _addedItem;

        public AddCommand(string text, bool isMultiline)
        {
            _text = text;
            _isMultiline = isMultiline;
        }

        public void Execute()
        {
            if (AppInfo.CurrentTodos == null)
                throw new AuthenticationException("Необходимо войти в профиль.");

            string finalText = _isMultiline ? ReadMultiline() : _text.Trim('"');
            if (string.IsNullOrWhiteSpace(finalText))
                throw new InvalidArgumentException("Текст задачи не может быть пустым.");
            
            _addedItem = new TodoItem(finalText);
            AppInfo.CurrentTodos.Add(_addedItem);
            AppInfo.UndoStack.Push(this);
            AppInfo.RedoStack.Clear();
            Console.WriteLine("Добавлено.");
        }

        public void Unexecute()
        {
            if (_addedItem != null && AppInfo.CurrentTodos != null)
            {
                int lastIndex = AppInfo.CurrentTodos.Count;
                if (lastIndex > 0)
                {
                    AppInfo.CurrentTodos.Delete(lastIndex);
                    Console.WriteLine("Добавление задачи отменено.");
                }
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