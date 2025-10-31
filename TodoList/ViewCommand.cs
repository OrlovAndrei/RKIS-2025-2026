using System;

namespace TodoList
{
    public class ViewCommand : ICommand
    {
        public TodoList TodoList { get; set; }
        public bool ShowIndex { get; set; }
        public bool ShowDone { get; set; }
        public bool ShowDate { get; set; }

        public void Execute()
        {
            if (TodoList.IsEmpty())
            {
                Console.WriteLine("Список задач пуст.");
                return;
            }

            int indexWidth = 5, textWidth = 35, statusWidth = 12, dateWidth = 20;

            if (ShowIndex) Console.Write("| {0,-5} ", "N");
            Console.Write("| {0,-35} ", "Задача");
            if (ShowDone) Console.Write("| {0,-12} ", "Статус");
            if (ShowDate) Console.Write("| {0,-20} ", "Дата");
            Console.WriteLine("|");

            if (ShowIndex) Console.Write($"+{new string('-', indexWidth + 2)}");
            Console.Write($"+{new string('-', textWidth + 2)}");
            if (ShowDone) Console.Write($"+{new string('-', statusWidth + 2)}");
            if (ShowDate) Console.Write($"+{new string('-', dateWidth + 2)}");
            Console.WriteLine("+");

            for (int i = 0; i < TodoList.Count; i++)
            {
                var item = TodoList.GetItem(i);
                if (ShowIndex) Console.Write($"| {i + 1,-5} ");
                string taskText = item.Text.Length > 30 ? item.Text.Substring(0, 30) + "..." : item.Text;
                taskText = taskText.Replace("\n", " ");
                Console.Write($"| {taskText,-35} ");
                if (ShowDone)
                    Console.Write($"| {(item.IsDone ? "Сделано" : "Не сделано"),-12} ");
                if (ShowDate)
                    Console.Write($"| {item.LastUpdate:dd.MM.yyyy HH:mm:ss,-20} ");
                Console.WriteLine("|");
            }
        }
    }
}