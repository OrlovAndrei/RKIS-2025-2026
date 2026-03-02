using System;
using System.Collections.Generic;
using System.Threading.Tasks;
public class LoadCommand : ICommand
{
	public int Count { get; set; }
	public int Size { get; set; }

	public void Execute()
	{
		Console.WriteLine($"\nЗапуск {Count} параллельных загрузок (размер: {Size})...");
		int startLine = Console.CursorTop;
		for (int i = 0; i < Count; i++) Console.WriteLine();
		var tasks = new Task[Count];
		object consoleLock = new object();
		for (int i = 0; i < Count; i++)
		{
			int index = i;
			tasks[i] = Task.Run(async () =>
			{
				int currentProgress = 0;
				Random rnd = new Random();

				while (currentProgress <= Size)
				{
					lock (consoleLock)
					{
						Console.SetCursorPosition(0, startLine + index);

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
			});
		}
		Task.WaitAll(tasks);
		Console.SetCursorPosition(0, startLine + Count);
		Console.WriteLine("\nВсе загрузки завершены успешно!\n");
	}
}

