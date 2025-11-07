namespace Task;

public class CSVLine
{
    private readonly char Separation = '|';
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
    public string Get() => string.Join(Separation, Items);
    /// <summary>
	/// Метод для вычисления количества элементов строки
	/// </summary>
	/// <returns>Длина строки</returns>
	public int GetLength() => Items.Count;
}
