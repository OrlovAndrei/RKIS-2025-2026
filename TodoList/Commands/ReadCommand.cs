using System;
using Todolist.Exceptions;

namespace Todolist
{
    public class ReadCommand : ICommand
    {
        public int TaskNumber { get; private set; }

        public ReadCommand(int taskNumber)
        {
            TaskNumber = taskNumber;
        }

        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
                throw new AuthenticationException("Необходимо войти в профиль");

            var todoList = AppInfo.GetCurrentTodos();

            if (TaskNumber < 1 || TaskNumber > todoList.GetCount())
                throw new TaskNotFoundException($"Задача с номером {TaskNumber} не существует");

            TodoItem item = todoList.GetItem(TaskNumber - 1);
            Console.WriteLine("=== Полная информация о задаче ===");
            Console.WriteLine(item.GetFullInfo());
            Console.WriteLine("===================================");
        }

        public void Unexecute()
        {
            throw new NotImplementedException("Команда read не поддерживает отмену");
        }
    }
}