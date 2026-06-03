using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Commands
{
    public class LoadCommand : ICommand
    {
        private const int BarWidth = 20;
        private static readonly object _consoleLock = new object();
        private static readonly Random _random = new Random();

        private readonly int _downloadsCount;
        private readonly int _downloadSize;
        private int _startRow;

        public LoadCommand(int downloadsCount, int downloadSize)
        {
            _downloadsCount = downloadsCount;
            _downloadSize = downloadSize;
        }

        public void Execute()
        {
            RunAsync().Wait();
        }

        private async Task RunAsync()
        {
            _startRow = Console.CursorTop;

            for (int i = 0; i < _downloadsCount; i++)
            {
                Console.WriteLine();
            }

            var tasks = new List<Task>();
            for (int i = 0; i < _downloadsCount; i++)
            {
                int index = i;
                tasks.Add(DownloadAsync(index));
            }

            await Task.WhenAll(tasks);

            lock (_consoleLock)
            {
                Console.SetCursorPosition(0, _startRow + _downloadsCount);
                Console.WriteLine("Все загрузки завершены.");
            }
        }

        private async Task DownloadAsync(int index)
        {
            for (int progress = 0; progress <= _downloadSize; progress++)
            {
                int percent = progress * 100 / _downloadSize;
                UpdateProgressBar(index, percent);
                await Task.Delay(GetDelay());
            }
        }

        private void UpdateProgressBar(int index, int percent)
        {
            int row = _startRow + index;
            string bar = BuildProgressBar(percent);

            lock (_consoleLock)
            {
                Console.SetCursorPosition(0, row);
                Console.Write(bar.PadRight(Console.WindowWidth - 1));
            }
        }

        private string BuildProgressBar(int percent)
        {
            int filled = percent / 5;
            string completed = new string('#', filled);
            string pending = new string('-', BarWidth - filled);

            return $"Загрузка {percent.ToString().PadLeft(3)}% [{completed}{pending}] {percent}%";
        }

        private int GetDelay()
        {
            lock (_random)
            {
                return _random.Next(20, 120);
            }
        }
    }
}
