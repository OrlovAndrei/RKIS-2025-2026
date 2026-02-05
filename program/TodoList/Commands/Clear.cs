using static TodoList.Input;
using static TodoList.WriteToConsole;
namespace TodoList;

public partial class Commands
{
    public static int ClearAllFile(string? fileName = "")
    {
        IfNull("Введите название файла: ", ref fileName);
        if (AccessVerificationList(fileName!))
        {
            if (Bool($"Вы уверены что хотите очистить весь файл {fileName}?: "))
            {
                CSVFile fileCSV = new(fileName!);
                File.Delete(fileCSV.File.FullPath);
                return 1;
            }
            else
            {
                Console.WriteLine("Буде внимательны");
            }
        }
        else
        {
            ColorMessage("Вы не можете очистить файлы другого пользователя!");
        }
        return 0;
    }
    public static int ClearRow(string? fileName)
    {
        IfNull("Введите название файла: ", ref fileName);

        CSVFile fileCSV = new(fileName!);
        if (File.Exists(fileCSV.File.FullPath))
        {
            int w = WriteColumn(fileCSV.File.NameFile);
            string requiredData = GetStringWriteColumn(fileCSV.GetColumn(w));
            if (AccessVerificationOne(fileName!, requiredData!, w))
            {
                fileCSV.File.EditingRow(requiredData!, w);
                return 1;
            }
            else
            {
                ColorMessage("Вы не можете очистить пункты другого пользователя!");
            }
        }
        else
        {
            ColorMessage("Такого файла не существует: ", ConsoleColor.Yellow);
        }

        return 0;
    }
}