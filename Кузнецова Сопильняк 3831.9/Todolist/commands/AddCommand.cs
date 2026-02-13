namespace TodoList
{
    public sealed class AddCommand : ICommand
    {
        public string TaskText { get; set; }
        public bool Multiline { get; set; }
        public TodoList TodoList { get; set; }

        public void Execute()
        {
            string task = "";

            if (Multiline)
            {
                Console.WriteLine("Многострочный режим. Вводите строки задачи. Для завершения введите !end");
                string multilineTask = "";
                while (true)
                {
                    Console.Write("> ");
                    string line = Console.ReadLine();
                    if (line == null) continue;
                    if (line == "!end")
                    {
                        break;
                    }
                    if (!string.IsNullOrEmpty(multilineTask))
                    {
                        multilineTask += "\n";
                    }
                    multilineTask += line;
                }
                task = multilineTask.Trim();
            }
            else
            {
                task = TaskText;
            }

            if (string.IsNullOrWhiteSpace(task))
            {
                Console.WriteLine("Ошибка: Текст задачи не может быть пустым.");
                return;
            }

            TodoItem newItem = new TodoItem(task);
            TodoList.Add(newItem);

            Console.WriteLine($"Задача добавлена: \"{task}\"");
            FileManager.SaveTodos(TodoList, "data/todo.csv");
        }
    }
}