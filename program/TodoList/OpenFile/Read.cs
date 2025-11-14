using System.Text;
using static TodoList.WriteToConsole;
namespace TodoList;

public partial class OpenFile
{
    public void GetAllLine(out List<CSVLine> allText)
    {
        allText = [];
        List<string> allTextString = File.Exists(FullPath)
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
    public CSVFile GetLinePositionInRow(string dataFile, int positionInRow, int count = 1)
    {
        /*Возвращает строку если ее элемент по заданной позиции 
            соответствует введенным нами данным*/
        CSVFile fileCSV = new(NameFile);
        try
        {
            using (StreamReader reader = new(FullPath, Encoding.UTF8))
            {
                CSVLine line;
                int counter = 0;
                if (fileCSV.Title!.GetLength() > positionInRow)
                {
                    while ((line = new(reader.ReadLine())).GetLength() != 0)
                    {
                        if (counter < count && line[positionInRow]!.Contains(dataFile))
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
        }
        catch (Exception)
        {
            RainbowText("Разраб отдыхает, прошу понять", ConsoleColor.Red);
            RainbowText("^если что там ошибка чтения файла", ConsoleColor.Red);
        }
        return fileCSV;
    }
    public CSVLine GetLinePositionRow(int positionRow)
    {
        /*Возвращает строку если ее элемент по заданной позиции 
            соответствует введенным нами данным*/
        CSVLine lineCSV = new();
        if (File.Exists(FullPath))
        {
            using (StreamReader reader = new StreamReader(FullPath, Encoding.UTF8))
            {
                int numLine = 0;
                while ((lineCSV = new(reader.ReadLine())).GetLength() != 0)
                {
                    if (numLine == positionRow)
                    {
                        break;
                    }
                    ++numLine;
                }
            }
        }
        else
        {
            RainbowText($"Файл '{NameFile}' не найден");
        }
        return lineCSV!;
    }
    public int GetLengthFile()
    {
        int numLine = 1;
        try
        {
            if (File.Exists(FullPath))
            {
                using (StreamReader reader = new StreamReader(FullPath, Encoding.UTF8))
                {
                    while (reader.ReadLine() is not null)
                    {
                        ++numLine;
                    }
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
        return numLine;
    }
}