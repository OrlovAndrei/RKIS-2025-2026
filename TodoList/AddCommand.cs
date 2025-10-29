namespace TodoList
{
    public class AddCommand : BaseCommand
    {
        public string Text { get; set; }
        public bool IsMultiline { get; set; }

        public AddCommand(TodoList todoList, string text, bool isMultiline) : base(todoList)
        {
            Text = text;
            IsMultiline = isMultiline;
        }

        public override void Execute()
        {
            string finalText = IsMultiline ? ReadMultiline() : Text.Trim('\"');
            if (string.IsNullOrWhiteSpace(finalText))
            {
                System.Console.WriteLine("Текст пустой.");
                return;
            }
            TodoItem item = new TodoItem(finalText);
            TodoList.Add(item);
            System.Console.WriteLine("Добавлено.");
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
