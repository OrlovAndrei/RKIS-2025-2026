using System;
using System.Collections.Generic;
using System.Linq;

namespace TodoList
{
    public class TodoList
    {
        private List<TodoItem> tasks = new();

        public void Add(string text)
        {
            tasks.Add(new TodoItem(text));
        }

        public void Delete(int index)
        {
            if (index < 0 || index >= tasks.Count)
            {
                Console.WriteLine("Ошибка: некорректный номер задачи");
                return;
            }
            tasks.RemoveAt(index);
        }

        public void MarkDone(int index)
        {
            if (index < 0 || index >= tasks.Count)
            {
                Console.WriteLine("Ошибка: некорректный номер задачи");
                return;
            }
            tasks[index].MarkDone();
        }

        public void Update(int index, string newText)
        {
            if (index < 0 || index >= tasks.Count)
            {
                Console.WriteLine("Ошибка: некорректный номер задачи");
                return;
            }
            tasks[index].UpdateText(newText);
        }

        public void View(bool showIndex = false, bool showStatus = false, bool showDate = false, bool showAll = false)
        {
            if (tasks.Count == 0)
            {
                Console.WriteLine("Список задач пуст");
                return;
            }

            if (showAll)
            {
                showIndex = showStatus = showDate = true;
            }

            const int textWidth = 30;
            int indexWidth = Math.Max(3, tasks.Count.ToString().Length + 1);
            int statusWidth = 12;
            int dateWidth = 19;

            var headers = new List<string>();
            if (showIndex) headers.Add("Idx".PadRight(indexWidth));
            if (showStatus) headers.Add("Status".PadRight(statusWidth));
            if (showDate) headers.Add("Updated".PadRight(dateWidth));
            headers.Add("Task".PadRight(textWidth));

            Console.WriteLine(string.Join(" | ", headers));
            Console.WriteLine(new string('-', headers.Sum(h => h.Length + 3)));

            for (int i = 0; i < tasks.Count; i++)
            {
                var row = new List<string>();
                if (showIndex) row.Add((i + 1).ToString().PadRight(indexWidth));
                if (showStatus)
                {
                    string st = tasks[i].IsDone ? "выполнена" : "не выполнена";
                    row.Add(st.PadRight(statusWidth));
                }
                if (showDate)
                    row.Add(tasks[i].LastUpdate.ToString("yyyy-MM-dd HH:mm:ss").PadRight(dateWidth));

                string text = tasks[i].GetShortInfo(textWidth);
                row.Add(text.PadRight(textWidth));

                Console.WriteLine(string.Join(" | ", row));
            }
        }

        public void Read(int index)
        {
            if (index < 0 || index >= tasks.Count)
            {
                Console.WriteLine("Ошибка: некорректный номер задачи");
                return;
            }

            var t = tasks[index];
            Console.WriteLine($"Задача {index + 1}:\n{t.Text}\nСтатус: {(t.IsDone ? "выполнена" : "не выполнена")}\nДата изменения: {t.LastUpdate}");
        }

        public int Count()
        {
            return tasks.Count;
        }
    }
}