using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LoadCommand : ICommand
{
    public int DownloadsCount { get; set; }
    public int DownloadSize { get; set; }
    
    private static readonly object _consoleLock = new object();

    public void Execute()
    {
        try
        {
            RunAsync().Wait();
        }
        catch (AggregateException ex)
        {
            throw ex.InnerException ?? ex;
        }
    }

    private async Task RunAsync()
    {
        Console.WriteLine($"Запуск {DownloadsCount} загрузок размером {DownloadSize}...\n");

        int startRow = Console.CursorTop;
        for (int i = 0; i < DownloadsCount; i++)
        {
            Console.WriteLine();
        }
        var tasks = new List<Task>();
        for (int i = 0; i < DownloadsCount; i++)
        {
            int index = i;
            tasks.Add(DownloadAsync(index, startRow + index));
        }
        await Task.WhenAll(tasks);

        Console.WriteLine($"\nВсе загрузки завершены.");
    }

    private async Task DownloadAsync(int downloadIndex, int row)
    {
        var random = new Random();

        for (int progress = 0; progress <= DownloadSize; progress++)
        {
            int percent = (progress * 100) / DownloadSize;
            string bar = GetProgressBar(percent);

            lock (_consoleLock)
            {
                Console.SetCursorPosition(0, row);
                Console.Write($"Загрузка {downloadIndex + 1}: {bar}");
            }
            await Task.Delay(random.Next(10, 50));
        }
    }

    private string GetProgressBar(int percent)
    {
        int completed = percent / 5; 
        string bar = "[";
        
        for (int i = 0; i < 20; i++)
        {
            bar += i < completed ? "#" : " ";
        }
        
        bar += $"] {percent}%";
        return bar;
    }
}