using TodoList.Exceptions;

namespace TodoList.commands;

public class SearchCommand : ICommand
{
    public string? ContainsText { get; set; }
    public string? StartsWithText { get; set; }
    public string? EndsWithText { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public TodoStatus? Status { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; }
    public int? Top { get; set; }

    public void Execute()
    {
        if (!AppInfo.CurrentProfileId.HasValue)
            throw new AuthenticationException("Необходимо войти в профиль для поиска задач.");

        var todoList = AppInfo.GetCurrentTodoList();
        var query = todoList.items.AsEnumerable();

        if (!string.IsNullOrEmpty(ContainsText))
            query = query.Where(item => item.Text.Contains(ContainsText, StringComparison.OrdinalIgnoreCase));
        
        if (!string.IsNullOrEmpty(StartsWithText))
            query = query.Where(item => item.Text.StartsWith(StartsWithText, StringComparison.OrdinalIgnoreCase));
        
        if (!string.IsNullOrEmpty(EndsWithText))
            query = query.Where(item => item.Text.EndsWith(EndsWithText, StringComparison.OrdinalIgnoreCase));

        if (FromDate.HasValue)
            query = query.Where(item => item.LastUpdate >= FromDate.Value);
        
        if (ToDate.HasValue)
            query = query.Where(item => item.LastUpdate <= ToDate.Value);

        if (Status.HasValue)
            query = query.Where(item => item.Status == Status.Value);

        if (!string.IsNullOrEmpty(SortBy))
        {
            if (SortBy.Equals("text", StringComparison.OrdinalIgnoreCase))
            {
                query = SortDescending 
                    ? query.OrderByDescending(item => item.Text) 
                    : query.OrderBy(item => item.Text);
            }
            else if (SortBy.Equals("date", StringComparison.OrdinalIgnoreCase))
            {
                query = SortDescending 
                    ? query.OrderByDescending(item => item.LastUpdate) 
                    : query.OrderBy(item => item.LastUpdate);
            }
        }

        if (Top.HasValue && Top.Value > 0)
            query = query.Take(Top.Value);

        var results = query.ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("Ничего не найдено");
            return;
        }

        Console.WriteLine("№     Статус          Дата                 Задача");
        Console.WriteLine("--------------------------------------------------------");

        for (int i = 0; i < results.Count; i++)
        {
            var item = results[i];
            var index = (i + 1).ToString().PadRight(6);
            var status = item.Status.ToString().PadRight(16);
            var date = item.LastUpdate.ToString("dd.MM.yyyy HH:mm").PadRight(20);
            
            var preview = item.Text.Length <= 30 
                ? item.Text 
                : item.Text.Substring(0, 27) + "...";
            
            Console.WriteLine($"{index}{status}{date}{preview}");
        }
    }

    public void Unexecute() { }
}