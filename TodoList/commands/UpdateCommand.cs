using TodoList;

namespace TodoList
{
    public class UpdateCommand : ICommand
    {
        private readonly int _index;
        private readonly string _newText;
        private string _oldText;
        private TodoStatus _oldStatus;
        private DateTime _oldLastUpdate;

        public UpdateCommand(int index, string newText)
        {
            _index = index;
            _newText = newText;
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
                var item = AppInfo.CurrentTodos[_index - 1];
                
                // Сохраняем старые значения для отмены
                _oldText = item.Text;
                _oldStatus = item.Status;
                _oldLastUpdate = item.LastUpdate;
                
                // Обновляем текст
                AppInfo.CurrentTodos.UpdateText(_index, _newText);
                
                AppInfo.UndoStack.Push(this);
                AppInfo.RedoStack.Clear();
                
                Console.WriteLine($"Задача {_index} обновлена.");
                Console.WriteLine($"Было: {_oldText}");
                Console.WriteLine($"Стало: {_newText}");
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
                // Восстанавливаем старые значения
                var item = AppInfo.CurrentTodos[_index - 1];
                
                // Используем рефлексию или временно отключаем события для восстановления
                // Так как нет прямого метода для восстановления, используем прямое изменение
                var textField = typeof(TodoItem).GetProperty("Text");
                var statusField = typeof(TodoItem).GetProperty("Status");
                var lastUpdateField = typeof(TodoItem).GetProperty("LastUpdate");
                
                if (textField != null && textField.CanWrite)
                    textField.SetValue(item, _oldText);
                if (statusField != null && statusField.CanWrite)
                    statusField.SetValue(item, _oldStatus);
                if (lastUpdateField != null && lastUpdateField.CanWrite)
                    lastUpdateField.SetValue(item, _oldLastUpdate);
                
                Console.WriteLine($"Обновление задачи {_index} отменено. Восстановлен текст: {_oldText}");
            }
        }
    }
}