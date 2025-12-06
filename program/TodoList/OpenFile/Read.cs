using System.Text;
namespace TodoList;

public partial class OpenFile
{
    public void GetAllLine(out List<CSVLine> allText)
    {
        allText = [];
        List<string> allTextString = Exist()
        ? File.ReadAllText(FullPath).Split("\n").ToList<string>()
        : [];
        foreach (var line in allTextString)
        {
            if (line.Length != 0)
            {
                allText.Add(new CSVLine(line));
            }
        }
    }
    public CSVFile SearchLineOnDataInLine(string requiredData, int indexInLine, int count = 1)
    {
        /*Возвращает строку если ее элемент по заданной позиции 
            соответствует введенным нами данным*/
        CSVFile fileCSV = new(NameFile);
        using (StreamReader reader = new(FullPath, Encoding.UTF8))
        {
            CSVLine line;
            int counter = 0;
            if (fileCSV.Title!.Length() > indexInLine)
            {
                while ((line = new(reader.ReadLine())).Length() != 0)
                {
                    if (counter < count && line[indexInLine]!.Contains(requiredData))
                    {
                        fileCSV.AddObject(line);
                        ++counter;
                    }
                    else if (counter == count)
                    {
                        break;
                    }
                }
            }
        }
        return fileCSV;
    }
    public CSVLine GetLineOnPosition(int indexLine)
    {
        /*Возвращает строку если ее элемент по заданной позиции 
            соответствует введенным нами данным*/
        CSVLine lineCSV = new();
        using (StreamReader reader = new StreamReader(FullPath, Encoding.UTF8))
        {
            int numLine = 0;
            while ((lineCSV = new(reader.ReadLine())).Length() != 0)
            {
                if (numLine == indexLine)
                {
                    break;
                }
                ++numLine;
            }
        }
        return lineCSV!;
    }
    public int Length()
    {
        int numLine = 0;
        if (Exist())
        {
            using (StreamReader reader = new StreamReader(FullPath, Encoding.UTF8))
            {
                while (reader.ReadLine() is not null)
                {
                    ++numLine;
                }
            }
        }
        return numLine;
    }
}