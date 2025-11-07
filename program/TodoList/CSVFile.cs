namespace Task;
public class CSVFile
{
    public CSVLine? Title { get; set; }
    public CSVLine? DataType { get; set; }
    public List<CSVLine> Objects { get; private set; } = [];
    public OpenFile ConfigFile { get; private set; }
    public OpenFile StandardFile { get; private set; }
    public CSVFile(string fileName, List<CSVLine>? objs = null)
    {
        StandardFile = new(fileName);
        ConfigFile = new(fileName, TypeFile.Config);
        Title = GetTitleLine();
        DataType = GetDataType();
        Objects = objs ?? [];
    }
    public CSVFile(string fileName, CSVLine title, CSVLine dataType, List<CSVLine>? objs = null)
    {
        StandardFile = new(fileName);
        ConfigFile = new(fileName, TypeFile.Config);
        Title = title;
        DataType = dataType;
        Objects = objs ?? [];
    }
    public void AddObject(CSVLine obj) => Objects.Add(obj);
    public void AddObject(List<CSVLine> objs)
    {
        foreach (var obj in objs)
        {
            AddObject(obj);
        }
    }
    private CSVLine GetTitleLine() => GetFromDataType(ConfigFile, 0);
	private CSVLine GetDataType() => GetFromDataType(ConfigFile, 1);
	private CSVLine GetFromDataType(OpenFile file, int position)
	{
		CSVLine line = new();
		if (File.Exists(file.fullPath))
		{
			line = file.GetLinePositionRow(position);
		}
		return line;
    }
}