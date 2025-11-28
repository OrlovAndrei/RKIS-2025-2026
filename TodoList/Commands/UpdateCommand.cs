using System;

namespace Todolist
{
    public class UpdateCommand : ICommand
    {
        public int TaskNumber { get; private set; }
        public string NewText { get; private set; }
        public Todolist TodoList { get; private set; }
        private string _oldText;
        private TodoItem _updatedItem;

        public UpdateCommand(Todolist todoList, int taskNumber, string newText)
        {
            TodoList = todoList;
            TaskNumber = taskNumber;
            NewText = newText;
        }

        public void Execute()
        {
            _updatedItem = TodoList.GetItem(TaskNumber - 1);
            _oldText = _updatedItem.Text;

            if (NewText.StartsWith("\"") && NewText.EndsWith("\""))
            {
                NewText = NewText.Substring(1, NewText.Length - 2);
            }

            _updatedItem.UpdateText(NewText);
            Console.WriteLine($"Обновил задачу: \nБыло: Задача №{TaskNumber} \"{_oldText}\" \nСтало: Задача №{TaskNumber} \"{NewText}\"");
        }

        public void Unexecute()
        {
            if (_updatedItem != null && _oldText != null)
            {
                _updatedItem.UpdateText(_oldText);
                Console.WriteLine($"Отменено обновление задачи №{TaskNumber}. Восстановлен текст: {_oldText}");
            }
        }
    }
}