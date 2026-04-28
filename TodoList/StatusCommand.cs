using System;

namespace TodoList
{
    public class StatusCommand : ICommand
    {
        private readonly TodoList _todoList;
        private readonly string _todoFilePath;
        private readonly int _index;
        private readonly TodoStatus _newStatus;

        public StatusCommand(TodoList todoList, string todoFilePath, int index, TodoStatus newStatus)
        {
            _todoList = todoList;
            _todoFilePath = todoFilePath;
            _index = index;
            _newStatus = newStatus;
        }

        public void Execute()
        {
            TodoItem item = _todoList.GetItem(_index);
            if (item == null)
            {
                Console.WriteLine("Ошибка: Задача с таким индексом не найдена.");
                return;
            }

            item.ChangeStatus(_newStatus);
            FileManager.SaveTasks(_todoList, _todoFilePath);
            Console.WriteLine($"Статус задачи {_index} изменен на: {item.GetStatusString()}");
        }
    }
}