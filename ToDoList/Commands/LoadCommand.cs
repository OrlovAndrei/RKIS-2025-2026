using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Exceptions;

namespace TodoList.Commands
{
	public class LoadCommand : ICommand
	{
		private readonly int _downloadsCount;
		private readonly int _totalSize;
		private static readonly object _consoleLock = new object();

		public LoadCommand(string args)
		{
			string[] parts = args.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length < 2)
				throw new InvalidCommandException("Используйте: load <количество> <размер>");

			if (!int.TryParse(parts[0], out _downloadsCount) || !int.TryParse(parts[1], out _totalSize))
				throw new InvalidCommandException("Аргументы должны быть числами.");

			if (_downloadsCount <= 0 || _totalSize <= 0)
				throw new InvalidCommandException("Значения должны быть больше 0.");
		}

		public void Execute()
		{
			RunAsync().Wait();
		}

		private async Task RunAsync()
		{
			int startRow = Console.CursorTop;

			for (int i = 0; i < _downloadsCount; i++)
			{
				Console.WriteLine();
			}

			List<Task> tasks = new List<Task>();
			for (int i = 0; i < _downloadsCount; i++)
			{
				int index = i;
				tasks.Add(DownloadAsync(index, startRow + index));
			}

			await Task.WhenAll(tasks);

			Console.SetCursorPosition(0, startRow + _downloadsCount);
			Console.WriteLine("Все загрузки завершены.");
		}

		private async Task DownloadAsync(int index, int row)
		{
			Random rnd = new Random();

			for (int current = 0; current <= _totalSize; current++)
			{
				int percent = (current * 100) / _totalSize;
				string bar = GenerateProgressBar(index, percent);

				lock (_consoleLock)
				{
					Console.SetCursorPosition(0, row);
					Console.Write(bar);
				}

				await Task.Delay(rnd.Next(50, 200));
			}
		}

		private string GenerateProgressBar(int index, int percent)
		{
			int totalChunks = 20;
			int filledChunks = percent / 5;

			string filled = new string('#', filledChunks);
			string empty = new string('-', totalChunks - filledChunks);

			return $"Загрузка {index + 1}: [{filled}{empty}] {percent}%";
		}
	}
}
