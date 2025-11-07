namespace Task;

public class CSVObject
{
    public string? Title { get; private set; }
    public string? DataType { get; private set; }
    public string? Object { get; private set; }
    public CSVObject(string title, string dataType, string obj)
    {
        Title = title;
        DataType = dataType;
        Object = obj;
    }
}