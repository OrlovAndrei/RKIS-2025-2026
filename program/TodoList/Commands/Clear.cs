using static TodoList.Input;
using static TodoList.WriteToConsole;
namespace TodoList;

public partial class Commands
{
    public static int ClearAllFile(string? fileName = "")
    {
        IfNull("Введите название файла: ", ref fileName);
        if (Bool($"Вы уверены что хотите очистить весь файл {fileName}?"))
        {
            CSVFile fileCSV = new(fileName!);
            File.Delete(fileCSV.File.FullPath);
            return 1;
        }
        else
        {
            System.Console.WriteLine("Буде внимательны");
            return 0;
        }
    }
    public static int ClearRow(string? fileName, string? requiredData = "")
    {
        IfNull("Введите название файла: ", ref fileName);
        CSVFile fileCSV = new(fileName!);
        if (File.Exists(fileCSV.File.FullPath))
        {
            IfNull("Поиск: ", ref requiredData);
            fileCSV.File.EditingRow(requiredData!, WriteColumn(fileCSV.File.NameFile));
            return 1;
        }
        else
        {
            RainbowText("Такого файла не существует: ", ConsoleColor.Yellow);
            return 0;
        }
    }
}