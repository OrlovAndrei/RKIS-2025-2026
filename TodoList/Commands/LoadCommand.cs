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
			foreach (var e in ae.InnerExceptions)
			{
				if (e is InvalidArgumentException)
					throw e;
			}
			throw;
		}
	}

	private async Task RunAsync()
	{

		if (string.IsNullOrWhiteSpace(Argument))
		{
			throw new InvalidArgumentException("Аргументы не указаны. Пример использования: load 3 100");
		}

		string[] parts = Argument.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		if (parts.Length != 2)
		{
			throw new InvalidArgumentException("Неверный формат команды. Ожидалось два числа: load <количество> <размер>");
		}

		if (!int.TryParse(parts[0], out int count) || count <= 0)
		{
			throw new InvalidArgumentException($"Некорректное количество загрузок: '{parts[0]}'. Введите целое число больше 0.");
		}

		if (!int.TryParse(parts[1], out int size) || size <= 0)
		{
			throw new InvalidArgumentException($"Некорректный размер загрузки: '{parts[1]}'. Введите целое число больше 0.");
		}


		Console.WriteLine($"Запуск {count} параллельных загрузок (размер: {size})...");

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
					var random = new Random(Guid.NewGuid().GetHashCode());

					for (int progress = 0; progress <= size; progress++)
					{
						int delay = random.Next(20, 150);
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
				Console.WriteLine("Все загрузки успешно завершены.");
			}
		}
	}

	private void DrawProgressBar(int index, int current, int total, int startTop)
	{
		const int barWidth = 30;

		double percent = (double)current / total;
		int filledChars = (int)(percent * barWidth);

		if (filledChars > barWidth) filledChars = barWidth;
		if (filledChars < 0) filledChars = 0;

		string bar = "[" + new string('#', filledChars) + new string('.', barWidth - filledChars) + "]";
		string status = $"{bar} {(int)(percent * 100),3}% | Поток #{index + 1}";

		lock (_consoleLock)
		{
			try
			{
				int oldLeft = Console.CursorLeft;
				int oldTop = Console.CursorTop;

				Console.SetCursorPosition(0, startTop + index);
				Console.Write(status);

				Console.SetCursorPosition(oldLeft, oldTop);
			}
			catch (ArgumentOutOfRangeException)
			{
			
			}
		}
	}
}