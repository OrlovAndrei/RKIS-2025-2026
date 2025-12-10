namespace TodoList
{
    public class TodoList
    {
        private TodoItem[] tasks;
        private int count;
        private const int InitialCapacity = 2;
        private const int ShortTextLength = 30;

        public TodoList()
        {
            tasks = new TodoItem[InitialCapacity];
            count = 0;
        }

        public void Add(TodoItem item)
        {
            if (item == null)
            {
                return;
            }

            if (count >= tasks.Length)
            {
                tasks = IncreaseArray(tasks);
            }

            tasks[count] = item;
            count++;
        }

        public void Delete(int index)
        {
            if (index < 1 || index > count)
            {
                Console.WriteLine($"Ошибка: Неверный индекс. Допустимые значения от 1 до {count}.");
                return;
            }

            int actualIndex = index - 1;
            for (int i = actualIndex; i < count - 1; i++)
            {
                tasks[i] = tasks[i + 1];
            }

            tasks[count - 1] = null; 
            count--;
            Console.WriteLine($"Задача {index} удалена.");
        }

        public void View(bool showIndex, bool showStatus, bool showDate)
        {
            if (count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            int indexWidth = count.ToString().Length;
            if (indexWidth < 5) indexWidth = 5;
            int taskWidth = ShortTextLength;
            int statusWidth = 10;
            int dateWidth = 19;

            string header = "";
            if (showIndex) header += $"{"Инд",-indexWidth} ";
            header += $"{"Задача",-taskWidth} ";
            if (showStatus) header += $"{"Статус",-statusWidth} ";
            if (showDate) header += $"{"Дата",-dateWidth}";

            Console.WriteLine("Список задач:");

            if (header.Length > 0)
            {
                Console.WriteLine(header.TrimEnd());
                Console.WriteLine(new string('-', header.Length));
            }
            else
            {
                if (count > 0)
                {
                    Console.WriteLine(new string('-', taskWidth));
                }
            }

            for (int i = 0; i < count; i++)
            {
                TodoItem item = tasks[i];
                string output = "";

                if (item == null) continue;

                if (showIndex)
                {
                    output += $"{(i + 1),-indexWidth} ";
                }

                string taskText = item.Text ?? string.Empty;
                if (taskText.Length > taskWidth)
                {
                    taskText = taskText.Substring(0, taskWidth - 3) + "...";
                }
                output += $"{taskText,-taskWidth} ";

                if (showStatus)
                {
                    string statusText = item.IsDone ? "сделано" : "не сделано";
                    output += $"{statusText,-statusWidth} ";
                }

                if (showDate)
                {
                    string dateText = item.LastUpdate.ToString("yyyy-MM-dd HH:mm:ss");
                    output += $"{dateText,-dateWidth}";
                }

                Console.WriteLine(output.TrimEnd());
            }
        }

        public TodoItem GetItem(int index)
        {
            if (index < 1 || index > count)
            {
                return null;
            }
            return tasks[index - 1];
        }

        private TodoItem[] IncreaseArray(TodoItem[] currentTasks)
        {
            int newCapacity = currentTasks.Length * 2;
            TodoItem[] newTasks = new TodoItem[newCapacity];

            for (int i = 0; i < currentTasks.Length; i++)
            {
                newTasks[i] = currentTasks[i];
            }

            return newTasks;
        }
    }
}