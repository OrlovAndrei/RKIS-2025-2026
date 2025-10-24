namespace TodoList
{

    class TodoList
    {
        private TodoItem[] items;

        public TodoList()
        {
            items = new TodoItem[2];
        }

        public void Add(TodoItem item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = item;
                    return;
                }
            }
            IncreaseArray(items, item);
        }

        public void Delete(int index)
        {
            if (index < 0 || index >= items.Length || items[index] == null)
                return;

            items[index] = null;

            for (int i = index; i < items.Length - 1; i++)
            {
                items[i] = items[i + 1];
            }
            items[items.Length - 1] = null;
        }

        public void View(bool showIndex, bool showStatus, bool showDate)
        {
            int taskCount = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] != null)
                {
                    taskCount++;
                }
            }

            if (taskCount == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            int indexWidth = 6;
            int descriptionWidth = 33;
            int statusWidth = 10;
            int dateWidth = 16;

            Console.WriteLine("\n=== Список задач ===");

            string header = "";
            if (showIndex) header += "Индекс".PadRight(indexWidth);
            header += "Описание".PadRight(descriptionWidth);
            if (showStatus) header += "Статус".PadRight(statusWidth);
            if (showDate) header += "Дата изменения".PadRight(dateWidth);
            Console.WriteLine(header);

            string separator = "";
            if (showIndex) separator += new string('-', indexWidth);
            separator += new string('-', descriptionWidth);
            if (showStatus) separator += new string('-', statusWidth);
            if (showDate) separator += new string('-', dateWidth);
            Console.WriteLine(separator);

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null) continue;

                string line = "";

                if (showIndex)
                {
                    line += i.ToString().PadRight(indexWidth);
                }

                string shortInfo = items[i].GetShortInfo();
                string[] parts = shortInfo.Split('|');
                string shortDescription = parts[0].Trim();
                string statusText = parts.Length > 1 ? parts[1].Trim() : "";
                string dateText = parts.Length > 2 ? parts[2].Trim() : "";

                line += shortDescription.PadRight(descriptionWidth);

                if (showStatus)
                {
                    line += statusText.PadRight(statusWidth);
                }

                if (showDate)
                {
                    line += dateText.PadRight(dateWidth);
                }

                Console.WriteLine(line);
            }
            Console.WriteLine($"Всего задач: {taskCount}\n");
        }

        public TodoItem GetItem(int index)
        {
            if (index < 0 || index >= items.Length)
                return null;
            return items[index];
        }

        private void IncreaseArray(TodoItem[] items, TodoItem item)
        {
            int newSize = items.Length * 2;
            TodoItem[] newItems = new TodoItem[newSize];

            for (int i = 0; i < items.Length; i++)
            {
                newItems[i] = items[i];
            }

            newItems[items.Length] = item;
            this.items = newItems;
        }
    }
}