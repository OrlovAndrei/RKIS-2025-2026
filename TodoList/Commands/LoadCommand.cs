using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todolist.Exceptions;

namespace Todolist
{
    public class LoadCommand : ICommand
    {
        private readonly int _downloadsCount;
        private readonly int _downloadSize;
        private static readonly object _consoleLock = new object();

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
            if (!AppInfo.CurrentProfileId.HasValue)
            {
                Console.WriteLine("Ошибка: Для использования команды load необходимо войти в профиль.");
                return;
            }

            if (_downloadsCount <= 0 || _downloadSize <= 0)
            {
                Console.WriteLine("Ошибка: Количество и размер загрузок должны быть положительными числами.");
                return;
            }

            Console.WriteLine($"\nЗапуск {_downloadsCount} параллельных загрузок (размер {_downloadSize})...");
            Console.WriteLine();

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
            Console.WriteLine("Все загрузки завершены.");
        }

        private async Task DownloadAsync(int downloadIndex, int consoleRow)
        {
            var random = new Random();

            for (int currentStep = 0; currentStep <= _downloadSize; currentStep++)
            {
                int progressPercent = (currentStep * 100) / _downloadSize;
                string progressBar = BuildProgressBar(progressPercent);

                lock (_consoleLock)
                {
                    Console.SetCursorPosition(0, consoleRow);
                    Console.Write($"Загрузка {downloadIndex + 1}: {progressBar}");
                }

                await Task.Delay(random.Next(20, 100));
            }
        }

        private string BuildProgressBar(int percent)
        {
            const int barLength = 20;
            int filledLength = (percent * barLength) / 100;

            string bar = new string('#', filledLength) + new string('-', barLength - filledLength);
            return $"[{bar}] {percent}%".PadRight(30);
        }

        public void Unexecute()
        {
            throw new NotImplementedException("Команда load не поддерживает отмену");
        }
    }
}