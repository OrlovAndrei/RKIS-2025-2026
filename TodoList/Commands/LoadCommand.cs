using TodoApp.Exceptions;

namespace TodoApp.Commands
{
	public class LoadCommand : BaseCommand
	{
		private readonly int _downloadsCount;
		private readonly int _downloadSize;
		private static readonly object _consoleLock = new object();

		public LoadCommand(int downloadsCount, int downloadSize)
		{
			if (downloadsCount <= 0)
				throw new LoadCommandException("Количество скачиваний должно быть положительным числом.");
			if (downloadSize <= 0)
				throw new LoadCommandException("Размер скачивания должен быть положительным числом.");

			_downloadsCount = downloadsCount;
			_downloadSize = downloadSize;
		}
		public override void Execute()
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
			var tasks = new Task[_downloadsCount];
			for (int i = 0; i < _downloadsCount; i++)
			{
				tasks[i] = DownloadAsync(i, startRow + i);
			}

			await Task.WhenAll(tasks);
			Console.WriteLine("\nВсе загрузки завершены.");
		}

		private async Task DownloadAsync(int index, int row)
		{
			var random = new Random(Environment.TickCount * 31 + index);

			for (int progress = 0; progress <= _downloadSize; progress++)
			{
				double percentage = (double)progress / _downloadSize * 100;
				UpdateProgressBar(percentage, row);

				int delay = random.Next(50, 201);
				await Task.Delay(delay);
			}
		}

		private void UpdateProgressBar(double percentage, int row)
		{
			lock (_consoleLock)
			{
				int filledBars = (int)(percentage / 5);
				if (filledBars > 20) filledBars = 20;
				int emptyBars = 20 - filledBars;

				string bar = "[" +
						   new string('#', filledBars) +
						   new string('-', emptyBars) +
						   "] " + Math.Round(percentage, 0) + "%";

				Console.SetCursorPosition(0, row);
				Console.Write(bar);
			}
		}
	}
}
