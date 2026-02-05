using System;

namespace TodoList
{
	class ReadCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public int TaskIndex { get; set; }

		public void Execute()
		{
			try
			{
				TodoList.Read(TaskIndex);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
		}

		public void Unexecute()
		{
			// пустой метод
		}
	}
}
