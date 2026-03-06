using System;

namespace Todolist
{
	// Команда для отмены Undo
	public class UndoCommand : ICommand
	{
		public string Description => "Отмена последнего действия";

		public void Execute()
		{
			if (AppInfo.UndoStack.Count == 0)
			{
				Console.WriteLine("Нечего отменять");
				return;
			}

			// Берем последнюю команду из UndoStack
			ICommand lastCommand = AppInfo.UndoStack.Pop();

			// Отменяем её действие
			lastCommand.Unexecute();

			// Кладем в RedoStack
			AppInfo.RedoStack.Push(lastCommand);

			Console.WriteLine($"Undo: {lastCommand.Description}");
			Console.WriteLine($"В стеке Undo: {AppInfo.UndoStack.Count}, Redo: {AppInfo.RedoStack.Count}");
		}

		public void Unexecute()
		{
			// Undo для самой команды Undo? Прикольно
			Console.WriteLine("Нельзя отменить отмену напрямую, используй Redo");
		}
	}

	// Команда для повтора Redo
	public class RedoCommand : ICommand
	{
		public string Description => "Повтор отмененного действия";

		public void Execute()
		{
			if (AppInfo.RedoStack.Count == 0)
			{
				Console.WriteLine("Нечего повторять");
				return;
			}

			// Берем команду из RedoStack
			ICommand commandToRedo = AppInfo.RedoStack.Pop();

			// Выполняем её снова
			commandToRedo.Execute();

			// Возвращаем в UndoStack
			AppInfo.UndoStack.Push(commandToRedo);

			Console.WriteLine($"Redo: {commandToRedo.Description}");
			Console.WriteLine($"В стеке Undo: {AppInfo.UndoStack.Count}, Redo: {AppInfo.RedoStack.Count}");
		}

		public void Unexecute()
		{
			// А теперь еще и отмена для Redo, жесть
			Console.WriteLine("Нельзя отменить повтор, используй Undo");
		}
	}
}