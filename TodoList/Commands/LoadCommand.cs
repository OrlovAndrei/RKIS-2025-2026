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
		try
		{
			RunAsync().Wait();
		}
		catch (AggregateException ae)
		{
			foreach (var ex in ae.InnerExceptions)
			{
				if (ex is InvalidArgumentException || ex is InvalidCommandException)
					throw ex;
			}
			throw ae;
		}
	}

	private async Task RunAsync()
	{
		if (string.IsNullOrWhiteSpace(Argument))
			throw new InvalidArgumentException("Укажите параметры. Пример: load 3 100");

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


			for (int i = 0; i < count; i++)
			{
				int taskIndex = i;

				tasks.Add(Task.Run(async () =>
				{
					var rnd = new Random(Guid.NewGuid().GetHashCode());

					for (int progress = 0; progress <= size; progress++)
					{
						int delay = rnd.Next(20, 100);
						await Task.Delay(delay);

						DrawProgressBar(taskIndex, progress, size, startTop);
					}
				}));
			}

			await Task.WhenAll(tasks);
		}
		finally
		{
			lock (_consoleLock)
			{
				Console.SetCursorPosition(0, startTop + count);
				Console.CursorVisible = wasCursorVisible;
				Console.WriteLine("Все загрузки завершены!");
			}
		}
	}

	private void DrawProgressBar(int index, int current, int total, int startTop)
	{
		const int width = 30;
		double percent = (double)current / total;
		int filled = (int)(percent * width);

		if (filled > width) filled = width;

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