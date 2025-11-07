namespace Task;
// enum CSVLineType: byte
// {
//     Title = 0,
//     DataType = 1,
//     Line = 2
// }
public class CSVLineObj
{
    public List<CSVObject> Line { set; private get; } = [];
    public CSVLineObj(List<CSVObject> line)
    {
        Line = line;
    }
    public CSVLineObj() { }
    public void AddObject(CSVObject obj) => Line.Add(obj);
    public void AddObject(List<CSVObject> objs)
    {
        foreach (var obj in objs)
        {
            AddObject(obj);
        }
    }
    public CSVLine GetTitle()
    {
        CSVLine line = new();
        foreach (var obj in Line)
        {
            line.Items.Add(obj.Title!);
        }
        return line;
    }
    public CSVLine GetLine()
    {
        CSVLine line = new();
        foreach (var obj in Line)
        {
            line.Items.Add(obj.Object!);
        }
        return line;
    }
    public CSVLine GetDataType()
    {
        CSVLine line = new();
        foreach (var obj in Line)
        {
            line.Items.Add(obj.DataType!);
        }
        return line;
    }
}
