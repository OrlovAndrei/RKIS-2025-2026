using System;

namespace TodoList
{
	class UpdateCommand : ICommand
	{
		public TodoList TodoList { get; set; }
		public int TaskIndex { get; set; }
		public string NewText { get; set; }

		public string oldText;
		public int updatedIndex;


		public void Execute()
		{
			try
			{
				updatedIndex = TaskIndex;
				oldText = TodoList[TaskIndex].Text;

				TodoList[TaskIndex].UpdateText(NewText);
				Console.WriteLine($"Задача {TaskIndex + 1} обновлена: {NewText}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
		}

		public void Unexecute()
		{
			if (oldText != null)
			{
				TodoList[updatedIndex].UpdateText(oldText);
				Console.WriteLine($"Отменено изменение задачи: восстановлен предыдущий текст");
			}
		}
	}
}
