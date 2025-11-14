using System.Text;
using static TodoList.WriteToConsole;
namespace TodoList;

public partial class OpenFile
{
    public void ReIndexFile(bool message = false)
    {
        if (File.Exists(FullPath))
        {
            try
            {
                OpenFile tempFile = new(NameFile, TypeFile.IndexAndTemporary);
                CSVLine line;
                using (StreamReader reader = new StreamReader(FullPath, Encoding.UTF8))
                {
                    int numLine = 1;
                    while ((line = new(reader.ReadLine())).GetLength() != 0)
                    {
                        line[0] = numLine.ToString();
                        tempFile.WriteFile(line);
                        ++numLine;
                    }
                }
                using (StreamReader reader = new StreamReader(tempFile.FullPath, Encoding.UTF8))
                {
                    WriteFile(new CSVLine(reader.ReadLine() ?? ""), false);
                    while ((line = new(reader.ReadLine())).GetLength() != 0)
                    {
                        WriteFile(line);
                    }
                }
                File.Delete(tempFile.FullPath);
            }
            catch (Exception)
            {
                RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
            }
        }
        else { RainbowText($"Файл под названием {NameFile}, не найден.", ConsoleColor.Red); }
    }
    public void AddRowInFile(CSVFile fileCSV, bool message = true)
    {
        try
        {
            AddFirst(fileCSV);
            Input.RowOnTitleAndConfig(fileCSV, out CSVLine outLine);
            fileCSV.File.WriteFile(outLine);
            if (message) { RainbowText("Задание успешно записано", ConsoleColor.Green); }
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void EditingRow(string requiredData, string modifiedData, int indexColumn,
    int numberOfIterations = 1, int indexColumnWrite = -1)
    {
        if (indexColumnWrite == -1) { indexColumnWrite = indexColumn; }
        bool maxCounter = false;
        if (numberOfIterations == -1)
        {
            maxCounter = true;
        }
        int counter = 0;
        if (File.Exists(FullPath))
        {
            try
            {
                OpenFile tempFile = new(NameFile, TypeFile.Temporary);
                CSVLine line;
                using (StreamReader reader = new StreamReader(FullPath, Encoding.UTF8))
                {
                    while ((line = new(reader.ReadLine())).GetLength() != 0)
                    {
                        if ((counter < numberOfIterations || maxCounter) && line[indexColumn] == requiredData)
                        {
                            line[indexColumnWrite] = modifiedData;
                            tempFile.WriteFile(line);
                            ++counter;
                        }
                        else { tempFile.WriteFile(line); }
                    }
                    RainbowText($"Было перезаписано '{counter}' строк", ConsoleColor.Green);

                }
                using (StreamReader reader = new StreamReader(tempFile.FullPath, Encoding.UTF8))
                {
                    WriteFile(new CSVLine(reader.ReadLine() ?? ""), false);
                    while ((line = new(reader.ReadLine())).GetLength() != 0)
                    {
                        WriteFile(line);
                    }
                }
                File.Delete(tempFile.FullPath);
            }
            catch (Exception)
            {
                RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
            }
        }
        else { RainbowText($"Файл под названием {NameFile}, не найден.", ConsoleColor.Red); }
    }
    public void ClearRow(string requiredData, int indexColumn, int numberOfIterations = 1)
    {
        int counter = 0;
        if (File.Exists(FullPath))
        {
            try
            {
                OpenFile tempFile = new(NameFile, TypeFile.Temporary);
                CSVLine line;
                using (StreamReader reader = new StreamReader(FullPath, Encoding.UTF8))
                {
                    while ((line = new(reader.ReadLine())).GetLength() != 0)
                    {
                        if (counter < numberOfIterations && line[indexColumn] == requiredData)
                        {
                            ++counter;
                        }
                        else { tempFile.WriteFile(line); }
                    }
                    RainbowText($"Было перезаписано '{counter}' строк", ConsoleColor.Green);
                }
                using (StreamReader reader = new StreamReader(tempFile.FullPath, Encoding.UTF8))
                {
                    WriteFile(new CSVLine(reader.ReadLine() ?? ""), false);
                    while ((line = new(reader.ReadLine())).GetLength() != 0)
                    {
                        WriteFile(line);
                    }
                }
                File.Delete(tempFile.FullPath);
                ReIndexFile();
            }
            catch (Exception ex)
            {
                RainbowText("не найдено, что именно я тоже не знаю", ConsoleColor.Red);
                Console.WriteLine(ex);
            }
        }
        else { RainbowText($"Файл под названием {NameFile}, не найден.", ConsoleColor.Red); }
    }
}