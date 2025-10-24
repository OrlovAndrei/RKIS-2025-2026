using System;
using System.Collections.Generic;

namespace Todolist
{
    public class TodoList
    {
        private TodoItem[] items;
        private int count;

        public TodoList()
        {
            items = new TodoItem[2];
            count = 0;
        }

        public void Add(TodoItem item)
        {
            if (count >= items.Length)
            {
                IncreaseArray();
            }

            items[count] = item;
            count++;
        }

        public void Delete(int index)
        {
            if (index >= 0 && index < count)
            {
                // Сдвигаем элементы влево
                for (int i = index; i < count - 1; i++)
                {
                    items[i] = items[i + 1];
                }

                items[count - 1] = null;
                count--;
            }
        }

        public void View(bool showIndex, bool showStatus, bool showDate)
        {
            if (count == 0)
            {
                Console.WriteLine("Задач нет");
                return;
            }

            var tableData = new List<string[]>();

            // Заголовки
            var headers = new List<string>();
            if (showIndex) headers.Add("№");
            headers.Add("Задача");
            if (showStatus) headers.Add("Статус");
            if (showDate) headers.Add("Изменено");

            tableData.Add(headers.ToArray());

            // Данные
            for (int i = 0; i < count; i++)
            {
                var row = new List<string>();
                if (showIndex) row.Add((i + 1).ToString());

                string shortText = items[i].Text.Length > 30 ?
                    items[i].Text.Substring(0, 27) + "..." : items[i].Text;
                row.Add(shortText);

                if (showStatus) row.Add(items[i].IsDone ? "V Выполнена" : "X Не выполнена");
                if (showDate) row.Add(items[i].LastUpdate.ToString("dd.MM.yy HH:mm"));

                tableData.Add(row.ToArray());
            }

            PrintTable(tableData);
        }

        public TodoItem GetItem(int index)
        {
            if (index >= 0 && index < count)
                return items[index];
            return null;
        }

        public int Count => count;

        private void IncreaseArray()
        {
            int newSize = items.Length * 2;
            var newItems = new TodoItem[newSize];

            for (int i = 0; i < items.Length; i++)
            {
                newItems[i] = items[i];
            }

            items = newItems;
            Console.WriteLine($"Массив расширен до {newSize} элементов");
        }

        private void PrintTable(List<string[]> tableData)
        {
            int columns = tableData[0].Length;
            int[] columnWidths = new int[columns];

            // Вычисляем ширину колонок
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

            // Выводим таблицу
            foreach (var row in tableData)
            {
                for (int i = 0; i < columns; i++)
                {
                    Console.Write(row[i].PadRight(columnWidths[i] + 2));
                }
                Console.WriteLine();

                // Разделитель после заголовка
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
