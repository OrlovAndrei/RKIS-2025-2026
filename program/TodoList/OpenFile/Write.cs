using static TodoList.WriteToConsole;
using System.Text;

namespace TodoList;

public partial class OpenFile
{
    /// <summary>
    /// Добавить титульное оформление и типы данных к нему
    /// </summary>
    /// <param name="fileCSV">Объект класса 
    /// в котором хранится титульное оформление, 
    /// типы данных и название файла</param>
    /// <param name="overwrite">true - перезаписать имеющееся, false - не трогать имеющееся</param>
    public static void AddTitleAndDataType(CSVFile fileCSV, bool overwrite = false)
    {
        if (!File.Exists(fileCSV.ConfigFile.FullPath) || overwrite)
            fileCSV.ConfigFile.WriteFile([fileCSV.Title!, fileCSV.DataType!], false);
    }
    /// <summary>
    /// Запись строки в файл
    /// </summary>
    /// <param name="dataFile">Данные которые будут записаны</param>
    /// <param name="noRewrite">true - продолжить, false - перезаписать</param>
    public void WriteFile(CSVLine dataFile, bool noRewrite = true)
    {
        using (StreamWriter sw = new(FullPath, noRewrite, Encoding.UTF8))
        {
            sw.WriteLine(dataFile.GetString());
        }
    }
    /// <summary>
    /// Запись списка строк
    /// </summary>
    /// <param name="dataFiles">Список строк который будет записан в файл</param>
    /// <param name="noRewrite">true - продолжить, false - перезаписать</param>
    public void WriteFile(List<CSVLine> dataFiles, bool noRewrite = true)
    {
        foreach (var dataFile in dataFiles)
        {
            if (!noRewrite)
            {
                WriteFile(dataFile, noRewrite);
                noRewrite = true;
            }
            else
            {
                WriteFile(dataFile);
            }
        }
    }
    /// <summary>
    /// Интерактивное заполнение одной строки и запись ее в файл
    /// </summary>
    /// <param name="fileCSV"></param>
    /// <param name="message">true - отправить сообщение о успешной записи,
    /// false - не отправлять сообщение о успешной записи</param>
    public static void AddRowInFile(CSVFile fileCSV, bool message = true)
    {
        AddTitleAndDataType(fileCSV);
        Input.RowOnTitleAndConfig(fileCSV, out CSVLine outLine);
        fileCSV.File.WriteFile(outLine);
        if (message) { RainbowText("Задание успешно записано", ConsoleColor.Green); }
    }
}