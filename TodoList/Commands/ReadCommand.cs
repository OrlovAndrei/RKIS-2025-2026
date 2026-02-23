using System;
using Todolist.Exceptions;

namespace Todolist
{
    public class ReadCommand : ICommand
    {
        public int TaskNumber { get; private set; }
        public Todolist TodoList { get; private set; }

        public ReadCommand(Todolist todoList, int taskNumber)
        {
            TodoList = todoList;
            TaskNumber = taskNumber;
        }

        public void Execute()
        {
            try
            {
                if (TodoList == null)
                {
                    Console.WriteLine("Ошибка: список задач не инициализирован");
                    return;
                }

                int index = TaskNumber - 1;
                if (index < 0)
                {
                    Console.WriteLine("Номер задачи должен быть положительным числом");
                    return;
                }
                
                if (index >= TodoList.GetCount())
                {
                    Console.WriteLine($"Задача с номером {TaskNumber} не найдена");
                    return;
                }

                TodoItem item = TodoList.GetItem(index);
                Console.WriteLine(item.GetFullInfo());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении задачи: {ex.Message}");
            }
        }
    }
}