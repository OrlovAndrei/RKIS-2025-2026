using System;
using System.Collections.Generic;

namespace Todolist
{
    public class TodoList
    {
        private List<TodoItem> items;

        public TodoList()
        {
            items = new List<TodoItem>();
        }

        public void Add(TodoItem item)
        {
            items.Add(item);
        }

        public void Delete(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                items.RemoveAt(index);
            }
        }

        public void SetStatus(int index, TodoStatus status)
        {
            if (index >= 0 && index < items.Count)
            {
                items[index].SetStatus(status);
            }
        }

        public TodoItem GetItem(int index)
        {
            if (index >= 0 && index < items.Count)
                return items[index];
            return null;
        }

        public TodoItem this[int index]
        {
            get => items[index];
        }

        public IEnumerable<TodoItem> GetItems()
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }

        public int Count => items.Count;

        public void View(bool showIndex, bool showStatus, bool showDate)
        {
            if (items.Count == 0)
            {
                Console.WriteLine("Задач нет");
                return;
            }

            var tableData = new List<string[]>();

            var headers = new List<string>();
            if (showIndex) headers.Add("№");
            headers.Add("Задача");
            if (showStatus) headers.Add("Статус");
            if (showDate) headers.Add("Изменено");

            tableData.Add(headers.ToArray());

            int counter = 1;
            foreach (var item in items)
            {
                var row = new List<string>();
                if (showIndex) row.Add(counter.ToString());

                string shortText = item.Text.Length > 30 ?
                    item.Text.Substring(0, 27) + "..." : item.Text;
                row.Add(shortText);

                if (showStatus) row.Add(GetStatusText(item.Status));
                if (showDate) row.Add(item.LastUpdate.ToString("dd.MM.yy HH:mm"));

                tableData.Add(row.ToArray());
                counter++;
            }

            PrintTable(tableData);
        }

        private string GetStatusText(TodoStatus status)
        {
            return status switch
            {
                TodoStatus.NotStarted => "Не начато",
                TodoStatus.InProgress => "В процессе",
                TodoStatus.Completed => "Выполнено",
                TodoStatus.Postponed => "Отложено",
                TodoStatus.Failed => "Провалено",
                _ => "Неизвестно"
            };
        }

        private void PrintTable(List<string[]> tableData)
        {
            if (tableData.Count == 0) return;

            int columns = tableData[0].Length;
            int[] columnWidths = new int[columns];

            foreach (var row in tableData)
            {
                for (int i = 0; i < columns; i++)
                {
                    if (row[i].Length > columnWidths[i])
                    {
                        columnWidths[i] = row[i].Length;
                    }
                }
            }

            foreach (var row in tableData)
            {
                for (int i = 0; i < columns; i++)
                {
                    Console.Write(row[i].PadRight(columnWidths[i] + 2));
                }
                Console.WriteLine();

                if (row == tableData[0])
                {
                    for (int i = 0; i < columns; i++)
                    {
                        Console.Write(new string('-', columnWidths[i]) + "  ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
