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
            TodoItem item = todoList.GetItem(index);
            if (item == null)
            {
                System.Console.WriteLine("Задача с таким индексом не найдена.");
                return;
            }
            string finalText = isMultiline ? ReadMultiline() : text.Trim('"');
            if (string.IsNullOrWhiteSpace(finalText))
            {
                System.Console.WriteLine("Текст пустой.");
                return;
            }
            item.UpdateText(finalText);
            System.Console.WriteLine("Обновлено.");
        }

        private static string ReadMultiline()
        {
            System.Console.WriteLine("Ввод построчно, !end для конца:");
            string res = "";
            while (true)
            {
                System.Console.Write("> ");
                string l = System.Console.ReadLine();
                if (l == "!end") break;
                res += l + "\n";
            }
            return res.TrimEnd('\n');
        }
    }
}