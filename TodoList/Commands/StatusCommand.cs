using System;
using TodoApp;
using TodoApp.Commands;

namespace Todoapp.Commands
{
    public class StatusCommand : BaseCommand
    {
        public TodoList TodoList { get; set; }
        public int Index { get; set; }
        public TodoStatus? NewStatus { get; set; }
        public override void Execute()
        {
            if (TodoList == null)
            {
                Console.WriteLine("Ошибка: список задач не инициализирован.");
                return;
            }
            if (Index < 0 || Index >= TodoList.Count)
            {
                Console.WriteLine($"Ошибка: задача с номером {Index + 1} не существует.");
                return;
            }
            if (NewStatus == null)
            {
                Console.WriteLine("Ошибка: статус не указан или указан неверно.");
                return;
            }
            var task = TodoList[Index];
            var oldStatus = task.Status;
            task.Status = NewStatus.Value;
            Console.WriteLine($"Статус задачи '{task.Text}' изменен: {TodoItem.GetStatusDisplayName(oldStatus)} -> {TodoItem.GetStatusDisplayName(NewStatus.Value)}");
        }
    }
}