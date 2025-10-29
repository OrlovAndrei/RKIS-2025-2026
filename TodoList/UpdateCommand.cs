namespace TodoList
{
    public class UpdateCommand : BaseCommand
    {
        public int Index { get; set; }
        public string Text { get; set; }
        public bool IsMultiline { get; set; }

        public UpdateCommand(TodoList todoList, int index, string text, bool isMultiline) : base(todoList)
        {
            Index = index;
            Text = text;
            IsMultiline = isMultiline;
        }

        public override void Execute()
        {
            TodoItem item = TodoList.GetItem(Index);
            if (item == null)
            {
                System.Console.WriteLine("Задача с таким индексом не найдена.");
                return;
            }
            string finalText = IsMultiline ? ReadMultiline() : Text.Trim('"');
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