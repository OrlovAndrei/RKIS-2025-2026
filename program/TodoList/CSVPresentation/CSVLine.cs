namespace TodoList;

public class CSVLine
{
    private static readonly char Separation = '|';
    public List<string?> Items { get; set; } = [];
    public CSVLine(string? line)
    {
        if (line is not null)
        {
            Items = line.Split(Separation).ToList<string?>(); 
        }
    }
    public CSVLine(params List<string?> items)
    {
        Items = items;
    }
    public CSVLine() { }
    public string this[int index]
    {
        get
        {
            return Items[index]!;
        }
        set
        {
            Items[index] = value;
        }
    }
    public List<string> this[Range index]
    {
        get
        {
            return Items[index]!;
        }
    }
    public string GetString() => string.Join(Separation, Items);
    public string[] GetStringArray() => Items!.ToArray<string>();
    /// <summary>
	/// Метод для вычисления количества элементов строки
	/// </summary>
	/// <returns>Длина строки</returns>
	public int Length() => Items.Count;
}
