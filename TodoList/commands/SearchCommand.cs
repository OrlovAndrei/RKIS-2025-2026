using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoList.commands
{
    public class SearchCommand : ICommand
    {
        public string[] parts { get; set; }

        public void Execute()
        {
            if (!AppInfo.CurrentProfileId.HasValue)
            {
                Console.WriteLine("Ошибка: нет активного профиля");
                return;
            }

            if (parts.Length < 2)
            {
                ShowHelp();
                return;
            }

            var todoList = AppInfo.GetCurrentTodoList();
            if (todoList.Count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            var searchResults = PerformSearch(todoList);
            DisplayResults(searchResults);
        }

        private List<TodoItem> PerformSearch(TodoList todoList)
        {
            var results = new List<TodoItem>();
            var searchType = parts[1].ToLower();

            switch (searchType)
            {
                case "text":
                case "поиск":
                    results = SearchByText(todoList);
                    break;
                case "status":
                case "статус":
                    results = SearchByStatus(todoList);
                    break;
                case "date":
                case "дата":
                    results = SearchByDate(todoList);
                    break;
                case "all":
                case "все":
                    results = GetAllTasks(todoList);
                    break;
                default:
                    Console.WriteLine($"Неизвестный тип поиска: {searchType}");
                    ShowHelp();
                    break;
            }

            return results;
        }

        private List<TodoItem> SearchByText(TodoList todoList)
        {
            if (parts.Length < 3)
            {
                Console.WriteLine("Ошибка: укажите текст для поиска");
                Console.WriteLine("Использование: search text [текст]");
                return new List<TodoItem>();
            }

            var searchText = string.Join(" ", parts.Skip(2)).ToLower();
            var results = new List<TodoItem>();

            for (int i = 0; i < todoList.Count; i++)
            {
                var item = todoList.GetItem(i);
                if (item.Text.ToLower().Contains(searchText))
                {
                    results.Add(item);
                }
            }

            Console.WriteLine($"Найдено задач: {results.Count}");
            return results;
        }

        private List<TodoItem> SearchByStatus(TodoList todoList)
        {
            if (parts.Length < 3)
            {
                Console.WriteLine("Ошибка: укажите статус для поиска (done/pending)");
                Console.WriteLine("Использование: search status [done|pending]");
                return new List<TodoItem>();
            }

            var statusParam = parts[2].ToLower();
            bool? searchStatus = null;

            switch (statusParam)
            {
                case "done":
                case "выполнена":
                case "true":
                    searchStatus = true;
                    break;
                case "pending":
                case "невыполнена":
                case "false":
                    searchStatus = false;
                    break;
                default:
                    Console.WriteLine($"Неизвестный статус: {statusParam}. Используйте 'done' или 'pending'");
                    return new List<TodoItem>();
            }

            var results = new List<TodoItem>();
            for (int i = 0; i < todoList.Count; i++)
            {
                var item = todoList.GetItem(i);
                if (item.IsDone == searchStatus)
                {
                    results.Add(item);
                }
            }

            Console.WriteLine($"Найдено задач: {results.Count}");
            return results;
        }

        private List<TodoItem> SearchByDate(TodoList todoList)
        {
            if (parts.Length < 4)
            {
                Console.WriteLine("Ошибка: укажите тип даты и значение");
                Console.WriteLine("Использование: search date [today|yesterday|date|after|before] [значение]");
                return new List<TodoItem>();
            }

            var dateType = parts[2].ToLower();
            var results = new List<TodoItem>();

            switch (dateType)
            {
                case "today":
                case "сегодня":
                    var today = DateTime.Today;
                    results = SearchByDateRange(todoList, today, today.AddDays(1));
                    Console.WriteLine($"Задачи за сегодня ({today:dd.MM.yyyy})");
                    break;

                case "yesterday":
                case "вчера":
                    var yesterday = DateTime.Today.AddDays(-1);
                    results = SearchByDateRange(todoList, yesterday, yesterday.AddDays(1));
                    Console.WriteLine($"Задачи за вчера ({yesterday:dd.MM.yyyy})");
                    break;

                case "after":
                case "после":
                    if (DateTime.TryParse(parts[3], out var afterDate))
                    {
                        results = SearchByDateRange(todoList, afterDate, DateTime.MaxValue);
                        Console.WriteLine($"Задачи после {afterDate:dd.MM.yyyy}");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный формат даты. Используйте ГГГГ-ММ-ДД или ДД.ММ.ГГГГ");
                    }
                    break;

                case "before":
                case "до":
                    if (DateTime.TryParse(parts[3], out var beforeDate))
                    {
                        results = SearchByDateRange(todoList, DateTime.MinValue, beforeDate);
                        Console.WriteLine($"Задачи до {beforeDate:dd.MM.yyyy}");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный формат даты. Используйте ГГГГ-ММ-ДД или ДД.ММ.ГГГГ");
                    }
                    break;

                case "date":
                case "дата":
                    if (DateTime.TryParse(parts[3], out var exactDate))
                    {
                        results = SearchByDateRange(todoList, exactDate, exactDate.AddDays(1));
                        Console.WriteLine($"Задачи за {exactDate:dd.MM.yyyy}");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка: неверный формат даты. Используйте ГГГГ-ММ-ДД или ДД.ММ.ГГГГ");
                    }
                    break;

                default:
                    Console.WriteLine($"Неизвестный тип даты: {dateType}");
                    break;
            }

            Console.WriteLine($"Найдено задач: {results.Count}");
            return results;
        }

        private List<TodoItem> SearchByDateRange(TodoList todoList, DateTime startDate, DateTime endDate)
        {
            var results = new List<TodoItem>();
            for (int i = 0; i < todoList.Count; i++)
            {
                var item = todoList.GetItem(i);
                if (item.LastUpdate >= startDate && item.LastUpdate < endDate)
                {
                    results.Add(item);
                }
            }
            return results;
        }

        private List<TodoItem> GetAllTasks(TodoList todoList)
        {
            var results = new List<TodoItem>();
            for (int i = 0; i < todoList.Count; i++)
            {
                results.Add(todoList.GetItem(i));
            }
            Console.WriteLine($"Всего задач: {results.Count}");
            return results;
        }

        private void DisplayResults(List<TodoItem> results)
        {
            if (results.Count == 0)
            {
                Console.WriteLine("Задачи не найдены");
                return;
            }

            // Определяем ширину колонок
            int indexWidth = 6;
            int statusWidth = 12;
            int dateWidth = 19;
            int textWidth = 50;

            string topBorder = "┌" + new string('─', indexWidth) + "┬" +
                             new string('─', textWidth) + "┬" +
                             new string('─', statusWidth) + "┬" +
                             new string('─', dateWidth) + "┐";
            Console.WriteLine(topBorder);

            string header = "│ №".PadRight(indexWidth - 1) + "│" +
                          " Текст задачи".PadRight(textWidth - 1) + "│" +
                          " Статус".PadRight(statusWidth - 1) + "│" +
                          " Дата изменения".PadRight(dateWidth - 1) + "│";
            Console.WriteLine(header);

            string separator = "├" + new string('─', indexWidth) + "┼" +
                             new string('─', textWidth) + "┼" +
                             new string('─', statusWidth) + "┼" +
                             new string('─', dateWidth) + "┤";
            Console.WriteLine(separator);

            for (int i = 0; i < results.Count; i++)
            {
                string shortText = GetShortenedText(results[i].Text, textWidth - 3);
                string status = results[i].IsDone ? "✓ Выполнена" : "✗ Не выполнена";
                string date = results[i].LastUpdate.ToString("dd.MM.yyyy HH:mm");

                string row = "│ " + (i + 1).ToString().PadRight(indexWidth - 2) + "│" +
                           " " + shortText.PadRight(textWidth - 2) + "│" +
                           " " + status.PadRight(statusWidth - 2) + "│" +
                           " " + date.PadRight(dateWidth - 2) + "│";
                Console.WriteLine(row);
            }

            string bottomBorder = "└" + new string('─', indexWidth) + "┴" +
                                new string('─', textWidth) + "┴" +
                                new string('─', statusWidth) + "┴" +
                                new string('─', dateWidth) + "┘";
            Console.WriteLine(bottomBorder);
        }

        private string GetShortenedText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return text.Length <= maxLength ? text : text.Substring(0, maxLength - 3) + "...";
        }

        private void ShowHelp()
        {
            Console.WriteLine("Поиск задач:");
            Console.WriteLine("  search text [текст]         - поиск по тексту задачи");
            Console.WriteLine("  search status [done|pending] - поиск по статусу");
            Console.WriteLine("  search date today           - задачи на сегодня");
            Console.WriteLine("  search date yesterday       - задачи на вчера");
            Console.WriteLine("  search date date [дата]     - задачи за конкретную дату");
            Console.WriteLine("  search date after [дата]    - задачи после даты");
            Console.WriteLine("  search date before [дата]   - задачи до даты");
            Console.WriteLine("  search all                  - показать все задачи");
            Console.WriteLine("");
            Console.WriteLine("Примеры:");
            Console.WriteLine("  search text купить");
            Console.WriteLine("  search status done");
            Console.WriteLine("  search date today");
            Console.WriteLine("  search date after 2024-01-01");
        }
    }
}