using System;

namespace TodoList
{
	class ReadCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public int TaskIndex { get; set; }

		public void Execute()
		{
			if (TaskIndex < 0 || TaskIndex >= TodoList.Count)
			{
				throw new Exception("Ошибка: Задача с таким номером не найдена.");
			}
			try
			{
				TodoList.Read(TaskIndex);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
		}
	}
}