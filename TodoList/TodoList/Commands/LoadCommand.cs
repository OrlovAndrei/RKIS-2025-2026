using System;
using System.Collections.Generic;
using System.Threading.Tasks;
public class LoadCommand : ICommand
{
	public int Count { get; set; }
	public int Size { get; set; }
	private object _consoleLock = new object();
	private int _startRow;
	public void Execute()
	{
		RunAsync().Wait();
	}
	private async Task RunAsync()
	{
		_startRow = Console.CursorTop;
		for (int i = 0; i < Count; i++) Console.WriteLine();
		List<Task> tasks = new List<Task>();
		for (int i = 0; i < Count; i++)
		{
			tasks.Add(DownloadAsync(i));
		}
		await Task.WhenAll(tasks);
		Console.SetCursorPosition(0, _startRow + Count);
		Console.WriteLine("Все загрузки завершены.");
	}
	private async Task DownloadAsync(int index)
	{
		int currentProgress = 0;
		Random rnd = new Random();
		while (currentProgress <= Size)
		{
			lock (_consoleLock)
			{
				Console.SetCursorPosition(0, _startRow + index);

				double percentage = (double)currentProgress / Size;
				int barWidth = 20;
				int filledWidth = (int)(percentage * barWidth);
				string bar = new string('█', filledWidth) + new string('-', barWidth - filledWidth);
				Console.Write($"Загрузка #{index + 1}: [{bar}] {currentProgress}/{Size} ({(int)(percentage * 100)}%)");
			}
			if (currentProgress >= Size) break;
			currentProgress += rnd.Next(1, Math.Max(2, Size / 10));
			if (currentProgress > Size) currentProgress = Size;
			await Task.Delay(rnd.Next(200, 600));
		}
	}
}
