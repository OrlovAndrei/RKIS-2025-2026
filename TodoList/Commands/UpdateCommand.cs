namespace TodoList
{
    public class UpdateCommand : ICommand
    {
        private readonly TodoList todoList;
        private readonly int index;
        private readonly string text;
        private readonly bool isMultiline;

        public UpdateCommand(TodoList todoList, int index, string text, bool isMultiline)
        {
            this.todoList = todoList;
            this.index = index;
            this.text = text;
            this.isMultiline = isMultiline;
        }

        public void Execute()
        {
            if (index < 1 || index > todoList.Todos.Count)
            {
                Console.WriteLine("Задача с таким индексом не найдена.");
                return;
            }

            try
            {
                TodoItem item = todoList[index - 1];  
                string finalText = isMultiline ? ReadMultiline() : text.Trim('"');
                if (string.IsNullOrWhiteSpace(finalText))
                {
                    Console.WriteLine("Текст пустой.");
                    return;
                }
                item.UpdateText(finalText);
                Console.WriteLine("Обновлено.");
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Задача с таким индексом не найдена.");
            }
        }

        private static string ReadMultiline()
        {
            Console.WriteLine("Ввод построчно, !end для конца:");
            string res = "";
            while (true)
            {
                Console.Write("> ");
                string l = Console.ReadLine();
                if (l == "!end") break;
                res += l + "\n";
            }
            return res.TrimEnd('\n');
        }
    }
}
