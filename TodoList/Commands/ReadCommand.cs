using System;

namespace Todolist
{
    public class ReadCommand : ICommand
    {
        public int TaskNumber { get; private set; }
        public Todolist TodoList { get; private set; }
        private TodoItem _readItem;

        public ReadCommand(Todolist todoList, int taskNumber)
        {
            TodoList = todoList;
            TaskNumber = taskNumber;
        }

        public void Execute()
        {
            if (TaskNumber < 1 || TaskNumber > TodoList.GetCount())
            {
                Console.WriteLine($"Ошибка: задача с номером {TaskNumber} не существует");
                return;
            }

            _readItem = TodoList.GetItem(TaskNumber - 1);
            Console.WriteLine("=== Полная информация о задаче ===");
            Console.WriteLine(_readItem.GetFullInfo());
            Console.WriteLine("===================================");
        }

        public void Unexecute()
        {
            throw new NotImplementedException("Команда read не поддерживает отмену");
        }
    }
}