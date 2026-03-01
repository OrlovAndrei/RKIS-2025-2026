using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Exceptions;

namespace TodoList.Commands
{
    public class LoadCommand : ICommand
    {
        public string Arg { get; set; } = string.Empty;
        public string[] Flags { get; set; } = Array.Empty<string>();

        private int _downloadsCount;
        private int _fileSize;

        public void Execute()
        {
            ValidateArguments();
            RunAsync().Wait();
        }

        private void ValidateArguments()
        {
            var parts = Arg.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            if (parts.Length < 2)
            {
                throw new InvalidArgumentException("Недостаточно аргументов. Использование: load <количество_скачиваний> <размер_скачиваний>");
            }

            if (!int.TryParse(parts[0], out _downloadsCount) || _downloadsCount <= 0)
            {
                throw new InvalidArgumentException("Количество скачиваний должно быть положительным числом.");
            }

            if (!int.TryParse(parts[1], out _fileSize) || _fileSize <= 0)
            {
                throw new InvalidArgumentException("Размер скачиваний должен быть положительным числом.");
            }
        }

        private async Task RunAsync()
        {
            Console.WriteLine($"\nЗапуск {_downloadsCount} загрузок по {_fileSize} единиц...\n");

            int startRow = Console.CursorTop;

            for (int i = 0; i < _downloadsCount; i++)
            {
                Console.WriteLine();
            }

            var tasks = new List<Task>();
            
            for (int i = 0; i < _downloadsCount; i++)
            {
                int index = i;
                tasks.Add(DownloadAsync(index, startRow + index));
            }

            await Task.WhenAll(tasks);

            Console.SetCursorPosition(0, startRow + _downloadsCount);
            Console.WriteLine($"\nВсе загрузки завершены.");
        }

        private async Task DownloadAsync(int downloadIndex, int row)
        {
            var random = new Random();
            
            for (int progress = 0; progress <= _fileSize; progress++)
            {
                int percent = (progress * 100) / _fileSize;
                UpdateProgressBar(row, percent);
                
                if (progress < _fileSize)
                {
                    await Task.Delay(random.Next(50, 150));
                }
            }
        }

        private void UpdateProgressBar(int row, int percent)
        {
            lock (_consoleLock)
            {
                int barLength = 20;
                int filledLength = (percent * barLength) / 100;
                
                string bar = "[";
                bar += new string('#', filledLength);
                bar += new string('-', barLength - filledLength);
                bar += $"] {percent}%";
                
                Console.SetCursorPosition(0, row);
                Console.Write(bar.PadRight(Console.WindowWidth - 1));
            }
        }

        private static readonly object _consoleLock = new object();

        public void Unexecute()
        {
        }
    }
}