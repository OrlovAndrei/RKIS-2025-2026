using System;
namespace Todolist.Commands
{
    internal class ViewCommand : ICommand
    {
        public string Args { get; set; }
        public bool ShowIndex { get; set; }
        public bool ShowStatus { get; set; }
        public bool ShowDate { get; set; }

        public ViewCommand(string args)
        {
            Args = args ?? string.Empty;

            string argsLower = Args.ToLowerInvariant();

            if (Args.Contains("--all", StringComparison.OrdinalIgnoreCase) || argsLower.Contains("-a"))
            {
                ShowIndex = ShowStatus = ShowDate = true;
                return;
            }

            // Support combined short flags like -is, -ds, -dis, -ids etc.
            // We'll find any tokens starting with '-' and parse their characters.
            var tokens = Args.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var t in tokens)
            {
                if (!t.StartsWith("-"))
                    continue;

                // long flags already handled; skip tokens like "--index"
                if (t.StartsWith("--"))
                {
                    if (t.Equals("--index", StringComparison.OrdinalIgnoreCase)) ShowIndex = true;
                    if (t.Equals("--status", StringComparison.OrdinalIgnoreCase)) ShowStatus = true;
                    if (t.Equals("--update-date", StringComparison.OrdinalIgnoreCase)) ShowDate = true;
                    continue;
                }

                // short combined flags: parse each character after '-'
                for (int i = 1; i < t.Length; i++)
                {
                    var ch = char.ToLowerInvariant(t[i]);
                    switch (ch)
                    {
                        case 'i': ShowIndex = true; break;
                        case 's': ShowStatus = true; break;
                        case 'd': ShowDate = true; break;
                        case 'a': ShowIndex = ShowStatus = ShowDate = true; break;
                        default: break; // ignore unknown short flags
                    }
                }
            }
        }

        public void Execute()
        {
            AppInfo.Todos.View(ShowIndex, ShowStatus, ShowDate);
        }

        public void Unexecute()
        {
            // ViewCommand только отображает данные, отменять нечего
        }
    }
}
