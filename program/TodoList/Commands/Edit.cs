using static TodoList.Input;
using static TodoList.WriteToConsole;
namespace TodoList;

public partial class Commands
{
    public static int EditRow(string? fileName,
    string? requiredData = "", string? modifiedData = "",
    int indexColumnSearch = -1, int indexColumnWrite = -1)
    {
        IfNull("Введите название файла: ", ref fileName);
        CSVFile fileCSV = new(fileName!);
        if (File.Exists(fileCSV.File.FullPath))
        {
            if (indexColumnSearch == -1) { indexColumnSearch = WriteColumn(fileCSV.File.NameFile, 2); }
            if (indexColumnWrite == -1) { indexColumnWrite = indexColumnSearch; }
            RainbowText("Введите искомое значение:", ConsoleColor.Green);
            IfNullOnDataType(fileCSV, indexColumnSearch, ref requiredData);
            RainbowText("Введите новое значение:", ConsoleColor.Green);
            IfNullOnDataType(fileCSV, indexColumnWrite, ref modifiedData);
            fileCSV.File.EditingRow(
                requiredData: requiredData!,
                modifiedData: modifiedData!,
                indexColumn: indexColumnSearch,
                indexColumnWrite: indexColumnWrite); // 2 означает что мы пропускаем из вывода numbering и Bool
            return 1;
        }
        else
        {
            RainbowText("Такого файла не существует: ", ConsoleColor.Yellow);
            return 0;
        }
    }
    public static int EditBoolRow(string? fileName, string? requiredData = "")
    {
        IfNull("Введите название файла: ", ref fileName);
        CSVFile fileCSV = new(fileName!);
        if (File.Exists(fileCSV.File.FullPath))
        {
            IfNull("Поиск: ", ref requiredData);
            Key($"Введите на что {requiredData} поменять(true/false): ",
                out ConsoleKey key, ConsoleKey.T, ConsoleKey.F);
            string? modifiedData = key switch
            {
                ConsoleKey.T => true.ToString(),
                ConsoleKey.F => false.ToString(),
                _ => null
            };
            fileCSV.File.EditingRow(
                requiredData: requiredData!,
                modifiedData: modifiedData!,
                indexColumn: WriteColumn(fileCSV.File.NameFile),
                indexColumnWrite: 1); // 1 в indexColumnWrite это bool строка таска
            return 1;
        }
        else
        {
            RainbowText("Такого файла не существует: ", ConsoleColor.Yellow);
            return 0;
        }
    }
    public static int FixingIndexing(string? fileName)
    {
        IfNull("Введите название файла: ", ref fileName);
        OpenFile file = new(fileName!);
        file.ReIndexFile(true);
        return 1;
    }
}