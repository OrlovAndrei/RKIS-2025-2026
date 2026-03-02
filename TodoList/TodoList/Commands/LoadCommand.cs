using System;
using System.Collections.Generic;
using System.Threading.Tasks;
public class LoadCommand : ICommand
{
	public int Count { get; set; }
	public int Size { get; set; }
	public void Execute()
	{
		RunAsync().Wait();
	}
	private async Task RunAsync()
	{
		Console.WriteLine($"\nПодготовка {Count} загрузок...");
		int startLine = Console.CursorTop;
		for (int i = 0; i < Count; i++) Console.WriteLine();
		object consoleLock = new object();
		List<Task> downloadTasks = new List<Task>();
		for (int i = 0; i < Count; i++)
		{
			int taskIndex = i;
			downloadTasks.Add(Task.Run(async () =>
			{
				int currentProgress = 0;
				Random rnd = new Random();
				while (currentProgress <= Size)
				{
					lock (consoleLock)
					{
						Console.SetCursorPosition(0, startLine + taskIndex);
						double percentage = (double)currentProgress / Size;
						int barWidth = 30;
						int filledWidth = (int)(percentage * barWidth);
						string bar = new string('█', filledWidth) + new string('-', barWidth - filledWidth);
						Console.Write($"Загрузка #{taskIndex + 1}: [{bar}] {currentProgress}/{Size} ({(int)(percentage * 100)}%)    ");
					}
					if (currentProgress >= Size) break;
					currentProgress += rnd.Next(1, Math.Max(2, Size / 5));
					if (currentProgress > Size) currentProgress = Size;
					await Task.Delay(rnd.Next(300, 800));
				}
			}));
		}
		await Task.WhenAll(downloadTasks);
		Console.SetCursorPosition(0, startLine + Count);
		Console.WriteLine("Все загрузки завершены!\n");
	}
}
