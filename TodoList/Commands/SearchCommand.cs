using System;
using System.Collections.Generic;
using System.Linq;

namespace Todolist
{
    public class SearchCommand : ICommand
    {
        private readonly Dictionary<string, string> _flags;

        public SearchCommand(Dictionary<string, string> flags)
        {
            _flags = flags;
        }

        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
            {
                Console.WriteLine("Ошибка: необходимо войти в профиль");
                return;
            }

            var todoList = AppInfo.GetCurrentTodos();
            if (todoList.GetCount() == 0)
            {
                Console.WriteLine("Задачи отсутствуют");
                return;
            }

            IEnumerable<TodoItem> query = todoList;

            if (_flags.ContainsKey("--contains"))
                query = query.Where(item => item.Text.Contains(_flags["--contains"]));

            if (_flags.ContainsKey("--starts-with"))
                query = query.Where(item => item.Text.StartsWith(_flags["--starts-with"]));

            if (_flags.ContainsKey("--ends-with"))
                query = query.Where(item => item.Text.EndsWith(_flags["--ends-with"]));

            if (_flags.ContainsKey("--from"))
            {
                if (DateTime.TryParse(_flags["--from"], out DateTime fromDate))
                {
                    query = query.Where(item => item.LastUpdate.Date >= fromDate.Date);
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный формат даты для флага --from");
                    return;
                }
            }

            if (_flags.ContainsKey("--to"))
            {
                if (DateTime.TryParse(_flags["--to"], out DateTime toDate))
                {
                    query = query.Where(item => item.LastUpdate.Date <= toDate.Date);
                }
                else
                {
                    Console.WriteLine("Ошибка: неверный формат даты для флага --to");
                    return;
                }
            }

            if (_flags.ContainsKey("--status"))
            {
                if (Enum.TryParse<TodoStatus>(_flags["--status"], true, out TodoStatus status))
                {
                    query = query.Where(item => item.Status == status);
                }
                else
                {
                    Console.WriteLine($"Ошибка: неверный статус. Доступные статусы: {string.Join(", ", Enum.GetNames(typeof(TodoStatus)))}");
                    return;
                }
            }

            bool isDescending = _flags.ContainsKey("--desc");
            
            if (_flags.ContainsKey("--sort"))
            {
                string sortField = _flags["--sort"];
                if (sortField == "text")
                {
                    query = isDescending 
                        ? query.OrderByDescending(item => item.Text)
                        : query.OrderBy(item => item.Text);
                }
                else if (sortField == "date")
                {
                    query = isDescending 
                        ? query.OrderByDescending(item => item.LastUpdate)
                        : query.OrderBy(item => item.LastUpdate);
                }
                else
                {
                    Console.WriteLine("Ошибка: неверное поле для сортировки. Доступные значения: text, date");
                    return;
                }
            }

            if (_flags.ContainsKey("--top"))
            {
                if (int.TryParse(_flags["--top"], out int top) && top > 0)
                {
                    query = query.Take(top);
                }
                else
                {
                    Console.WriteLine("Ошибка: неверное значение для флага --top. Укажите положительное число");
                    return;
                }
            }

            var results = query.ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("Ничего не найдено");
                return;
            }

            Console.WriteLine("=== Результаты поиска ===");
            Console.WriteLine("Index | Text | Status | LastUpdate");
            Console.WriteLine(new string('-', 70));

            int index = 1;
            foreach (var item in results)
            {
                string shortText = item.Text.Replace("\n", " ");
                if (shortText.Length > 30)
                    shortText = shortText.Substring(0, 30) + "...";

                Console.WriteLine($"{index,5} | {shortText,-30} | {item.Status,-12} | {item.LastUpdate:yyyy-MM-dd}");
                index++;
            }

            Console.WriteLine($"\nНайдено задач: {results.Count}");
        }

        public void Unexecute()
        {
            throw new NotImplementedException("Команда search не поддерживает отмену");
        }
    }
}