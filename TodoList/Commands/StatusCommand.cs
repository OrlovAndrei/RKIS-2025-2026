using System;
using System.Collections.Generic;
using TodoList;
namespace TodoApp.Commands
{
	public class StatusCommand : BaseCommand
	{
		public TodoList TodoList { get; set; }
		public int Index { get; set; }
		public TodoStatus? NewStatus { get; set; }
		private TodoStatus _oldStatus;

		public StatusCommand()
		{
			TodoList = AppInfo.Todos;
		}

		public StatusCommand(int index, TodoStatus newStatus) : this()
		{
			Index = index;
			NewStatus = newStatus;
		}

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

			var task = TodoList.GetItem(Index);
			_oldStatus = task.Status;
			task.Status = NewStatus.Value;
			Console.WriteLine($"Статус задачи '{task.Text}' изменен: {GetStatusDisplayName(_oldStatus)} -> {GetStatusDisplayName(NewStatus.Value)}");
		}

		public override void Unexecute()
		{
			if (TodoList != null && Index >= 0 && Index < TodoList.Count)
			{
				var task = TodoList.GetItem(Index);
				task.Status = _oldStatus;
				Console.WriteLine($"Отменено изменение статуса задачи: {task.Text} -> {GetStatusDisplayName(_oldStatus)}");
			}
		}

		private string GetStatusDisplayName(TodoStatus status)
		{
			return status switch
			{
				TodoStatus.NotStarted => "Не начата",
				TodoStatus.InProgress => "В процессе",
				TodoStatus.Completed => "Выполнена",
				TodoStatus.Postponed => "Отложена",
				TodoStatus.Failed => "Провалена",
				_ => "Неизвестно"
			};
		}
	}
}