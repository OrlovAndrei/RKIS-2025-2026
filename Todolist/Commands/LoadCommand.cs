using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todolist.Commands
{
    internal class LoadCommand : ICommand
    {
        private const int BarSegments = 20;
        private const int MinDelayMs = 30;
        private const int MaxDelayMs = 120;

        private static readonly object _consoleLock = new object();

        private readonly int _downloadsCount;
        private readonly int _downloadSize;

        public LoadCommand(int downloadsCount, int downloadSize)
        {
            _downloadsCount = downloadsCount;
            _downloadSize = downloadSize;
        }

        public void Execute()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private async Task RunAsync()
        {
            int startRow;
            lock (_consoleLock)
            {
                startRow = Console.CursorTop;
                for (int i = 0; i < _downloadsCount; i++)
                {
                    Console.WriteLine();
                }
            }

            var tasks = new List<Task>(_downloadsCount);
            for (int i = 0; i < _downloadsCount; i++)
            {
                int index = i;
                int row = startRow + i;
                tasks.Add(DownloadAsync(index, row));
            }

            await Task.WhenAll(tasks);

            lock (_consoleLock)
            {
                Console.SetCursorPosition(0, startRow + _downloadsCount);
                Console.WriteLine("Все загрузки завершены.");
            }
        }

        private async Task DownloadAsync(int index, int row)
        {
            for (int loaded = 0; loaded <= _downloadSize; loaded++)
            {
                int percent = loaded * 100 / _downloadSize;
                string bar = BuildProgressBar(percent);
                WriteProgressRow(row, $"#{index + 1} {bar}");
                await Task.Delay(Random.Shared.Next(MinDelayMs, MaxDelayMs + 1));
            }
        }

        private static string BuildProgressBar(int percent)
        {
            int filled = percent * BarSegments / 100;
            int empty = BarSegments - filled;
            return $"[{new string('#', filled)}{new string('-', empty)}] {percent,3}%";
        }

        private static void WriteProgressRow(int row, string text)
        {
            lock (_consoleLock)
            {
                Console.SetCursorPosition(0, row);
                Console.Write(text.PadRight(40));
            }
        }
    }
}
