namespace TodoList;

public class CSVFile
{
    public CSVLine? Title { get; set; } = new();
    public CSVLine? DataType { get; set; } = new();
    public List<CSVLine> Objects { get; private set; } = [];
    public OpenFile ConfigFile { get; private set; }
    public OpenFile File { get; private set; }
    public int? WidthFile;
    public CSVFile(string fileName, List<CSVLine>? objs = null)
    {
        File = new(fileName);
        ConfigFile = new(fileName, TypeFile.Config);
        Title = GetTitleLine();
        DataType = GetDataType();
        Objects = objs ?? [];
    }
    public CSVFile(string fileName, CSVLine title, CSVLine dataType, List<CSVLine>? objs = null)
    {
        File = new(fileName);
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
    /// <summary>
    /// Получает данные титульного оформления
    /// </summary>
    /// <returns></returns>
    private CSVLine GetTitleLine() => GetFromDataType(ConfigFile, 0);
    /// <summary>
    /// Получает типы данных объектов титульного оформления
    /// </summary>
    /// <returns></returns>
    private CSVLine GetDataType() => GetFromDataType(ConfigFile, 1);
    /// <summary>
    /// Получает CSV строку по заданной позиции
    /// </summary>
    /// <param name="file"></param>
    /// <param name="position">номер получаемой строки</param>
    /// <returns></returns>
    private CSVLine GetFromDataType(OpenFile file, int position)
    {
        CSVLine line = new();
        if (System.IO.File.Exists(file.FullPath))
        {
            line = file.GetLineOnPosition(position);
        }
        return line;
    }
    /// <summary>
    /// Вычисляет высоту CSV файла
    /// </summary>
    /// <returns></returns>
    public int GetHeight() => File.Length();
    /// <summary>
    /// Вычисляет ширину CSV файла
    /// </summary>
    /// <returns></returns>
    public int GetWidth() => Title!.Length();
    /// <summary>
    /// Получает объект по его координатам
    /// </summary>
    /// <param name="w">Ширина, считается слева на право</param>
    /// <param name="h">Высота, считается сверху вниз</param>
    /// <returns></returns>
    public string GetVoles(int w, int h) =>
        File.GetLineOnPosition(h)[w];
    public List<string> GetColumn(int position)
    {
        List<string> column = new();
        if (position < GetWidth())
        {
            for (int i = 0; i < GetHeight(); ++i)
            {
                column.Add(GetVoles(w: position, h: i));
            }
        }
        return column;
    }
}