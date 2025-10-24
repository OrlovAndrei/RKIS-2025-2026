using System;

namespace TodoList
{
    class TodoList
    {
        private TodoItem[] items;
        private int count;

        public TodoList(int capacity = 10)
        {
            items = new TodoItem[capacity];
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
            if (index < 0 || index >= count)
            {
                Console.WriteLine("Ошибка: неверный индекс задачи");
                return;
            }

            for (int i = index; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }
            count--;
            Console.WriteLine($"Задача {index + 1} удалена");
        }

        public void View(bool showIndex = false, bool showDone = false, bool showDate = false)
        {
            if (count == 0)
            {
                Console.WriteLine("Задач нет");
                return;
            }

            Console.WriteLine("Ваши задачи:");
            int indexWidth = 8;
            int statusWidth = 14;
            int dateWidth = 16;
            int taskWidth = 36;

            string header = "|";
            string separator = "|";

            if (showIndex)
            {
                header += " Индекс".PadRight(indexWidth) + " |";
                separator += new string('-', indexWidth + 2) + "|";
            }
            if (showDone)
            {
                header += " Статус".PadRight(statusWidth) + " |";
                separator += new string('-', statusWidth + 2) + "|";
            }
            if (showDate)
            {
                header += " Дата".PadRight(dateWidth) + " |";
                separator += new string('-', dateWidth + 2) + "|";
            }
            header += " Задача".PadRight(taskWidth) + " |";
            separator += new string('-', taskWidth + 2) + "|";
            Console.WriteLine(header);
            Console.WriteLine(separator);

            for (int i = 0; i < count; i++)
            {
                string row = "|";

                if (showIndex)
                    row += $" {i + 1}".PadRight(indexWidth) + " |";

                if (showDone)
                    row += $" {(items[i].IsDone ? "Сделано" : "Не сделано")}".PadRight(statusWidth) + " |";

                if (showDate)
                    row += $" {items[i].LastUpdate:dd.MM.yyyy HH:mm}".PadRight(dateWidth) + " |";

                string shortTask = items[i].Text.Replace("\n", " ").Replace("\r", "");
                if (shortTask.Length > taskWidth - 2)
                    shortTask = shortTask.Substring(0, taskWidth - 3) + "...";
                row += $" {shortTask}".PadRight(taskWidth) + " |";
                Console.WriteLine(row);
            }
        }

        public void Read(int index)
        {
            if (index < 0 || index >= count)
            {
                Console.WriteLine("Ошибка: неверный индекс задачи");
                return;
            }

            Console.WriteLine($"Задача {index + 1}:");
            Console.WriteLine(items[index].GetFullInfo());
        }
        public int Count
        {
            get { return count; }
        }

        public TodoItem GetItem(int index)
        {
            if (index < 0 || index >= count)
            {
                throw new ArgumentException("Неверный индекс задачи");
            }
            return items[index];
        }

        private void IncreaseArray()
        {
            TodoItem[] newItems = new TodoItem[items.Length * 2];
            for (int i = 0; i < count; i++)
            {
                newItems[i] = items[i];
            }
            items = newItems;
        }
    }
}
