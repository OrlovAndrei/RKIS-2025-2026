using System;
using System.Text;

namespace TodoList
{
    public class AddCommand : ICommand
    {
        private readonly TodoList _todoList;
        public string TaskText { get; set; }
        public bool IsMultiline { get; set; }

        public AddCommand(TodoList todoList)
        {
            _todoList = todoList;
        }

        public void Execute()
        {
            if (string.IsNullOrEmpty(TaskText))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return;
            }

            TodoItem newItem = new TodoItem(TaskText);
            _todoList.Add(newItem);

            string previewText = TaskText.Split('\n', 2)[0];
            Console.WriteLine($"Задача добавлена: {previewText}{(TaskText.Contains('\n') ? "..." : "")}");
        }
    }
}