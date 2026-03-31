using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TodoList
{
    public class TodoList : IEnumerable<TodoItem>
    {
        private List<TodoItem> _tasks;

        // События
        public event Action<TodoItem>? OnTodoAdded;
        public event Action<TodoItem>? OnTodoDeleted;
        public event Action<TodoItem>? OnTodoUpdated;
        public event Action<TodoItem>? OnStatusChanged;

        public TodoList()
        {
            _tasks = new List<TodoItem>();
        }
        
        public int Count => _tasks.Count;

        public void Add(TodoItem item)
        {
            _tasks.Add(item);
            OnTodoAdded?.Invoke(item);
        }

        public void Insert(int index, TodoItem item)
        {
            if (index >= 0 && index <= _tasks.Count)
            {
                _tasks.Insert(index, item);
                OnTodoAdded?.Invoke(item);
            }
        }

        public bool Delete(int index)
        {
            if (index < 1 || index > _tasks.Count) 
                return false;
            
            var item = _tasks[index - 1];
            _tasks.RemoveAt(index - 1);
            OnTodoDeleted?.Invoke(item);
            return true;
        }

        public TodoItem this[int index]
        {
            get
            {
                if (index < 0 || index >= _tasks.Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return _tasks[index];
            }
        }

        public void SetStatus(int index, TodoStatus status)
        {
            if (index < 1 || index > _tasks.Count) 
                return;
            
            var item = _tasks[index - 1];
            item.SetStatus(status);
            OnStatusChanged?.Invoke(item);
        }

        public void View(bool showIndex, bool showStatus, bool showDate)
        {
            if (_tasks.Count == 0)
            {
                Console.WriteLine("Нет задач.");
                return;
            }

            // Определяем ширину колонок
            int indexWidth = 8;
            int statusWidth = 14;
            int dateWidth = 17;
            int textWidth = 50;
            
            // Вычисляем доступную ширину консоли
            int totalWidth = 0;
            if (showIndex) totalWidth += indexWidth + 3;
            if (showStatus) totalWidth += statusWidth + 3;
            if (showDate) totalWidth += dateWidth + 3;
            totalWidth += textWidth + 3;
            
            if (totalWidth > Console.WindowWidth && Console.WindowWidth > 50)
            {
                int extraWidth = totalWidth - Console.WindowWidth;
                textWidth = textWidth - extraWidth - 5;
                if (textWidth < 20) textWidth = 20;
            }
            
            // Формируем заголовок
            string header = "";
            if (showIndex) header += "Индекс".PadRight(indexWidth) + " | ";
            if (showStatus) header += "Статус".PadRight(statusWidth) + " | ";
            header += "Текст".PadRight(textWidth);
            if (showDate) header += " | " + "Дата изменения".PadRight(dateWidth);
            
            Console.WriteLine(header);
            Console.WriteLine(new string('-', header.Length));

            // Выводим каждую задачу
            for (int i = 0; i < _tasks.Count; i++)
            {
                var task = _tasks[i];
                string row = "";
                
                // Индекс
                if (showIndex)
                {
                    row += (i + 1).ToString().PadRight(indexWidth) + " | ";
                }
                
                // Статус с символом
                if (showStatus)
                {
                    string statusSymbol = GetStatusSymbol(task.Status);
                    string statusText = statusSymbol + " " + task.Status.ToString();
                    row += statusText.PadRight(statusWidth) + " | ";
                }
                
                // Текст (обрезаем до нужной длины)
                string displayText = task.Text.Replace("\n", " ");
                if (displayText.Length > textWidth)
                {
                    displayText = displayText.Substring(0, textWidth - 3) + "...";
                }
                row += displayText.PadRight(textWidth);
                
                // Дата
                if (showDate)
                {
                    string formattedDate = task.LastUpdate.ToString("dd.MM.yyyy HH:mm");
                    row += " | " + formattedDate.PadRight(dateWidth);
                }
                
                Console.WriteLine(row);
            }
            
            Console.WriteLine(new string('-', header.Length));
            Console.WriteLine($"\nВсего задач: {_tasks.Count}");
        }
        
        private string GetStatusSymbol(TodoStatus status)
        {
            switch (status)
            {
                case TodoStatus.Completed:
                    return "✓";
                case TodoStatus.InProgress:
                    return "▶";
                case TodoStatus.Postponed:
                    return "⏸";
                case TodoStatus.Failed:
                    return "✗";
                default:
                    return "○";
            }
        }

        public void UpdateText(int index, string newText)
        {
            if (index < 1 || index > _tasks.Count) 
                return;
            
            var item = _tasks[index - 1];
            item.UpdateText(newText);
            OnTodoUpdated?.Invoke(item);
        }

        public IEnumerator<TodoItem> GetEnumerator()
        {
            foreach (var task in _tasks)
            {
                yield return task;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}