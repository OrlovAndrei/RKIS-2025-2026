namespace TodoList.Commands
{
    public class ViewCommand : ICommand
    {
        public bool ShowAll { get; set; } = false;
        public bool ShowIndex { get; set; } = false;
        public bool ShowStatus { get; set; } = false;
        public bool ShowDate { get; set; } = false;
        public string[] Flags { get; set; } = Array.Empty<string>();
        public TodoList TodoList { get; set; } = null!;

        public void Execute()
        {
            var flagList = new List<string>();

            if (ShowAll) flagList.Add("all");
            if (ShowIndex) flagList.Add("index");
            if (ShowStatus) flagList.Add("status");
            if (ShowDate) flagList.Add("update-date");

            if (Flags != null)
            {
                flagList.AddRange(Flags);
            }

            TodoList.ViewTasks(flagList.ToArray());

        }
    }
}