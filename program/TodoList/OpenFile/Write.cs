using static TodoList.WriteToConsole;
using System.Text;

namespace TodoList;

public partial class OpenFile
{
    public static void AddFirst(CSVFile fileCSV, bool overwrite = false)
    {
        if (!File.Exists(fileCSV.ConfigFile.FullPath) || overwrite)
            using (FileStream fs = new(fileCSV.ConfigFile.FullPath, FileMode.OpenOrCreate,
            FileAccess.Write, FileShare.Read))
            {
                fileCSV.ConfigFile.WriteFile(fileCSV.Title!, false);
                fileCSV.ConfigFile.WriteFile(fileCSV.DataType!);
            }
    }
    public void WriteFile(CSVLine dataFile, bool noRewrite = true)
    {
        /*Запись строки в конец файла при условии что 
            аргумент "noRewrite" равен true, а иначе файл будет перезаписан*/
        try
        {
            using (StreamWriter sw = new(FullPath, noRewrite, Encoding.UTF8))
            {
                sw.WriteLine(dataFile.GetString());
            }
        }
        catch (Exception)
        {
            RainbowText("В мире произошло что то плохое", ConsoleColor.Red);
        }
    }
    public void WriteFile(List<CSVLine> dataFiles, bool noRewrite = true)
    {
        /*Запись строки в конец файла при условии что 
            аргумент "noRewrite" равен true, а иначе файл будет перезаписан*/
        foreach (var dataFile in dataFiles)
        {
            WriteFile(dataFile);
        }
    }
}