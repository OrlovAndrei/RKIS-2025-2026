using System;

class ViewCommand : ICommand
{
    public TodoList TodoList { get; set; }
    public string Args { get; set; }
    public bool ShowIndex { get; set; }
    public bool ShowStatus { get; set; }
    public bool ShowDate { get; set; }

    public ViewCommand(TodoList todoList, string args)
    {
        TodoList = todoList;
        Args = args ?? string.Empty;
        
        string argsLower = Args.ToLowerInvariant();

        if (Args.Contains("--all", StringComparison.OrdinalIgnoreCase) ||
            argsLower.Contains("-a"))
        {
            ShowIndex = ShowStatus = ShowDate = true;
        }
        else
        {
            // Обработка комбинаций сокращенных флагов: -is, -ds, -dis
            // Проверяем комбинации всех трех флагов: -dis, -ids, -dsi
            if (argsLower.Contains("-dis") || argsLower.Contains("-ids") || argsLower.Contains("-dsi"))
            {
                ShowIndex = true;
                ShowStatus = true;
                ShowDate = true;
            }
            else
            {
                // Комбинация двух флагов: -is (index + status)
                if (argsLower.Contains("-is"))
                {
                    ShowIndex = true;
                    ShowStatus = true;
                }
                
                // Комбинация двух флагов: -ds (status + date)
                if (argsLower.Contains("-ds"))
                {
                    ShowStatus = true;
                    ShowDate = true;
                }
            }

            // Обработка отдельных флагов (проверяем как полные, так и сокращенные)
            if (Args.Contains("--index", StringComparison.OrdinalIgnoreCase) || argsLower.Contains("-i"))
                ShowIndex = true;
                
            if (Args.Contains("--status", StringComparison.OrdinalIgnoreCase) || argsLower.Contains("-s"))
                ShowStatus = true;
                
            if (Args.Contains("--update-date", StringComparison.OrdinalIgnoreCase) || argsLower.Contains("-d"))
                ShowDate = true;
        }
    }

    public void Execute()
    {
        TodoList.View(ShowIndex, ShowStatus, ShowDate);
    }
}

