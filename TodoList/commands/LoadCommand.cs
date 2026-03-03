using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Exceptions;

namespace TodoList.commands;

public class LoadCommand : ICommand
{
    public required string[] parts { get; set; }

    public void Execute()
    {
        // Аргументы уже проверены в парсере, но можно оставить дополнительную проверку
        if (parts.Length < 3)
            throw new InvalidArgumentException("Укажите количество загрузок и размер. Использование: load <количество> <размер>");

        if (!int.TryParse(parts[1], out var downloadsCount) || downloadsCount <= 0)
            throw new InvalidArgumentException("Количество загрузок должно быть положительным целым числом.");

        if (!int.TryParse(parts[2], out var downloadSize) || downloadSize <= 0)
            throw new InvalidArgumentException("Размер загрузки должен быть положительным целым числом.");

        RunAsync(downloadsCount, downloadSize).Wait();
    }

    private async Task RunAsync(int downloadsCount, int downloadSize)
    {
        int startRow = Console.CursorTop;

        for (int i = 0; i < downloadsCount; i++)
        {
            Console.WriteLine();
        }

        var tasks = new List<Task>();
        for (int i = 0; i < downloadsCount; i++)
        {
            int index = i;
            tasks.Add(DownloadAsync(index, startRow + index, downloadSize));
        }

        await Task.WhenAll(tasks);

        Console.SetCursorPosition(0, startRow + downloadsCount);
        Console.WriteLine("Все загрузки завершены.");
    }

    private async Task DownloadAsync(int taskIndex, int row, int totalSteps)
    {
        const int barLength = 20;
        var random = new Random();

        for (int currentStep = 0; currentStep <= totalSteps; currentStep++)
        {
            int percent = (int)((double)currentStep / totalSteps * 100);

            int filledChars = (int)((double)percent / 100 * barLength);
            string bar = new string('#', filledChars).PadRight(barLength, '-');

            string output = $"[{bar}] {percent}%";

            lock (_consoleLock)
            {
                Console.SetCursorPosition(0, row);
                Console.Write(output.PadRight(Console.WindowWidth));
            }

            int delay = random.Next(50, 150);
            await Task.Delay(delay);
        }
    }

    private static readonly object _consoleLock = new object();

    public void Unexecute() { }
}