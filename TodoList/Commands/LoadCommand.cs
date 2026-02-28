using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class LoadCommand : ICommand
{
	public string Argument { get; set; }

	private static readonly object _consoleLock = new object();

	public void Execute()
	{
		if (string.IsNullOrWhiteSpace(Argument))
			throw new InvalidArgumentException("Укажите параметры. Пример: load 3 100 (3 загрузки, размер 100)");

		string[] parts = Argument.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		if (parts.Length != 2)
			throw new InvalidArgumentException("Неверный формат. Ожидалось: load <количество> <размер>");

		if (!int.TryParse(parts[0], out int count) || count <= 0)
			throw new InvalidArgumentException("Количество загрузок должно быть положительным числом.");

		if (!int.TryParse(parts[1], out int size) || size <= 0)
			throw new InvalidArgumentException("Размер загрузки должен быть положительным числом.");

		Console.WriteLine($"Запуск {count} параллельных загрузок (размер {size})...");

		int startTop = Console.CursorTop;

		bool wasCursorVisible = Console.CursorVisible;
		Console.CursorVisible = false;

		for (int i = 0; i < count; i++)
		{
			Console.WriteLine();
		}

		try
		{
			var tasks = new List<Task>();
			var random = new Random();

			for (int i = 0; i < count; i++)
			{
				int taskIndex = i;

				tasks.Add(Task.Run(async () =>
				{
					for (int progress = 0; progress <= size; progress++)
					{
						int delay = random.Next(10, 100);
						await Task.Delay(delay);

						DrawProgressBar(taskIndex, progress, size, startTop);
					}
				}));
			}

			Task.WaitAll(tasks.ToArray());
		}
		finally
		{
			Console.SetCursorPosition(0, startTop + count);
			Console.CursorVisible = wasCursorVisible;
			Console.WriteLine("Все загрузки завершены!");
		}
	}

	private void DrawProgressBar(int index, int current, int total, int startTop)
	{
		const int width = 30;
		double percent = (double)current / total;
		int filled = (int)(percent * width);

		string bar = "[" + new string('#', filled) + new string('.', width - filled) + "]";
		string info = $"{bar} {(int)(percent * 100),3}% | Поток #{index + 1}";

		lock (_consoleLock)
		{
			try
			{
				int oldLeft = Console.CursorLeft;
				int oldTop = Console.CursorTop;

				Console.SetCursorPosition(0, startTop + index);
				Console.Write(info);

				Console.SetCursorPosition(oldLeft, oldTop);
			}
			catch (ArgumentOutOfRangeException)
			{

			}
		}
	}
}