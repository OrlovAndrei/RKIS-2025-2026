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
                _oldText = item.Text;
                string finalText = _isMultiline ? ReadMultiline() : _text.Trim('"');
                
                if (string.IsNullOrWhiteSpace(finalText))
                {
                    Console.WriteLine("Текст пустой.");
                    return;
                }
                
                item.UpdateText(finalText);
                AppInfo.UndoStack.Push(this);
                AppInfo.RedoStack.Clear();
                Console.WriteLine("Обновлено.");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Задача с таким индексом не найдена.");
            }
        }

        public void Unexecute()
        {
            if (_index >= 1 && _index <= AppInfo.CurrentTodos.Count && AppInfo.CurrentTodos != null)
            {
                AppInfo.CurrentTodos[_index - 1].UpdateText(_oldText);
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