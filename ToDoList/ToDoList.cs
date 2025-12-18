namespace MyTodoApp
{
    public class TaskCollection
    {
        private TaskItem[] _tasks;
        private int _count;
        private const int InitialSize = 2;
        private const int ShortDescLength = 30;

        public TaskCollection()
        {
            _tasks = new TaskItem[InitialSize];
            _count = 0;
        }

        public void Add(TaskItem task)
        {
            if (task == null) return;

            if (_count >= _tasks.Length)
            {
                _tasks = ExpandArray(_tasks);
            }

            _tasks[_count] = task;
            _count++;
        }

        public void RemoveAt(int position)
        {
            if (position < 1 || position > _count)
            {
                Console.WriteLine($"Ошибка: индекс вне диапазона от 1 до {_count}");
                return;
            }

            int actualIndex = position - 1;

            for (int i = actualIndex; i < _count - 1; i++)
            {
                _tasks[i] = _tasks[i + 1];
            }

            _tasks[_count - 1] = null;
            _count--;

            Console.WriteLine($"Задача под номером {position} удалена.");
        }

        public void Display(bool showPosition, bool showStatus, bool showUpdateDate)
        {
            if (_count == 0)
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            int posWidth = _count.ToString().Length;
            if (posWidth < 5) posWidth = 5;
            int descWidth = ShortDescLength;
            int statusWidth = 10;
            int dateWidth = 19;

            string header = "";
            if (showPosition) header += $"{"№",-posWidth} ";
            header += $"{"Задача",-descWidth} ";
            if (showStatus) header += $"{"Статус",-statusWidth} ";
            if (showUpdateDate) header += $"{"Обновлено",-dateWidth}";

            Console.WriteLine("Текущие задачи:");

            if (!string.IsNullOrWhiteSpace(header))
            {
                Console.WriteLine(header.TrimEnd());
                Console.WriteLine(new string('-', header.Length));
            }
            else
            {
                Console.WriteLine(new string('-', descWidth));
            }

            for (int i = 0; i < _count; i++)
            {
                var task = _tasks[i];
                if (task == null) continue;

                string line = "";

                if (showPosition)
                    line += $"{(i + 1),-posWidth} ";

                string desc = task.Description ?? "";
                if (desc.Length > descWidth)
                    desc = desc.Substring(0, descWidth - 3) + "...";

                line += $"{desc,-descWidth} ";

                if (showStatus)
                {
                    string statusText = task.IsCompleted ? "Выполнено" : "В работе";
                    line += $"{statusText,-statusWidth} ";
                }

                if (showUpdateDate)
                {
                    string dateText = task.LastModified.ToString("yyyy-MM-dd HH:mm:ss");
                    line += $"{dateText,-dateWidth}";
                }

                Console.WriteLine(line.TrimEnd());
            }
        }

        public TaskItem GetAt(int position)
        {
            if (position < 1 || position > _count)
                return null;

            return _tasks[position - 1];
        }

        private TaskItem[] ExpandArray(TaskItem[] current)
        {
            int newSize = current.Length * 2;
            TaskItem[] newArray = new TaskItem[newSize];

            for (int i = 0; i < current.Length; i++)
            {
                newArray[i] = current[i];
            }

            return newArray;
        }
    }
}