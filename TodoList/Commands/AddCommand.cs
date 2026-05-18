using System;

namespace TodoList
{
    public class AddCommand : ICommand
    {
        public bool IsMultiline { get; private set; }
        public string TaskText { get; private set; }
        public TodoList TodoList { get; private set; }

        public AddCommand(TodoList todoList, string taskText, bool isMultiline = false)
        {
            TodoList = todoList;
            TaskText = taskText;
            IsMultiline = isMultiline;
        }

        public void Execute()
        {
            if (IsMultiline)
            {
                string multilineText = "";
                string line;

                while (true)
                {
                    Console.Write("> ");
                    line = Console.ReadLine();
                    if (line == "!end")
                        break;
                    multilineText += line + "\n";
                }

                TaskText = multilineText.Trim();
            }

            TodoItem newItem = new TodoItem(TaskText);
            TodoList.Add(newItem);
            Console.WriteLine("Задача добавлена");
        }
    }
}