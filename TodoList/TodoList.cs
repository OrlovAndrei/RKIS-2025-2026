using System;

namespace TodoList
{
    public class TodoList
    {
        private List<TodoItem> tasks = new List<TodoItem>();

        public void AddTask(string line, string[] flags)
        {
            string text;

            if (flags.Contains("multiline"))
            {
                Console.WriteLine("Многострочный ввод (введите !end для завершения):");
                var lines = new List<string>();
                while (true)
                {
                    Console.Write("> ");
                    string? input = Console.ReadLine();
                    if (input == null || input.Trim() == "!end") break;
                    if (!string.IsNullOrWhiteSpace(input))
                        lines.Add(input);
                }
                text = string.Join("\n", lines);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    Console.WriteLine("Ошибка: не введён текст задачи");
                    return;
                }
                text = line.Trim('"', '\'');
            }

            tasks.Add(new TodoItem(text));
            Console.WriteLine("Задача добавлена!");
        }

        public void MarkTaskDone(string line)
        {
            if (!int.TryParse(line, out int idx))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            idx--;
            if (idx < 0 || idx >= tasks.Count)
            {
                Console.WriteLine("Ошибка: некорректный номер задачи");
                return;
            }

            tasks[idx].MarkDone();  // ✅ заменили прямое изменение свойств
            Console.WriteLine("Задача выполнена");
        }

        public void DeleteTask(string line)
        {
            if (!int.TryParse(line, out int idx))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            idx--;
            if (idx < 0 || idx >= tasks.Count)
            {
                Console.WriteLine("Ошибка: некорректный номер задачи");
                return;
            }

            tasks.RemoveAt(idx);
            Console.WriteLine("Задача удалена");
        }

        public void UpdateTask(string line)
        {
            var parts = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 2 || !int.TryParse(parts[0], out int idx))
            {
                Console.WriteLine("Ошибка: укажите номер задачи и текст");
                return;
            }

            idx--;
            if (idx < 0 || idx >= tasks.Count)
            {
                Console.WriteLine("Ошибка: некорректный номер задачи");
                return;
            }

            tasks[idx].UpdateText(parts[1].Trim('"', '\''));  // ✅ используем метод
            Console.WriteLine("Задача обновлена");
        }

        public void ViewTasks(string[] flags)
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            bool showIndex = flags.Contains("index") || flags.Contains("all");
            bool showStatus = flags.Contains("status") || flags.Contains("all");
            bool showDate = flags.Contains("update-date") || flags.Contains("all");

            Console.WriteLine("---------------------------------------------------------------");

            for (int i = 0; i < tasks.Count; i++)
            {
                var item = tasks[i];
                string text = item.GetShortInfo();
                string line = "";

                if (showIndex) line += $"{i + 1}. ";
                if (showStatus) line += item.IsDone ? "[выполнена] " : "[не выполнена] ";
                if (showDate) line += $"({item.LastUpdate:yyyy-MM-dd HH:mm}) ";

                Console.WriteLine(line + text);
            }

            Console.WriteLine("---------------------------------------------------------------");
        }

        public void ReadTask(string line)
        {
            if (!int.TryParse(line, out int idx))
            {
                Console.WriteLine("Ошибка: укажите номер задачи");
                return;
            }

            idx--;
            if (idx < 0 || idx >= tasks.Count)
            {
                Console.WriteLine("Ошибка: некорректный номер задачи");
                return;
            }

            var item = tasks[idx];
            Console.WriteLine($"\nЗадача {idx + 1}:");
            Console.WriteLine(item.Text);
            Console.WriteLine($"Статус: {(item.IsDone ? "выполнена" : "не выполнена")}");
            Console.WriteLine($"Дата изменения: {item.LastUpdate}\n");
        }
    }
}