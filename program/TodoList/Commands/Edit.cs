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
            ColorMessage("Введите искомое значение:", ConsoleColor.Green);
            IfNullOnDataType(fileCSV, indexColumnSearch, ref requiredData);
            ColorMessage("Введите новое значение:", ConsoleColor.Green);
            IfNullOnDataType(fileCSV, indexColumnWrite, ref modifiedData);
            if (AccessVerificationOne(fileName!, requiredData!, indexColumnSearch))
            {
                fileCSV.File.EditingRow(
                    requiredData: requiredData!,
                    modifiedData: modifiedData!,
                    indexColumn: indexColumnSearch,
                    indexColumnWrite: indexColumnWrite); // 2 означает что мы пропускаем из вывода numbering и Bool
                return 1;
            }
            else
            {
                ColorMessage("Вы не можете редактировать файлы других пользователей!");
            }
        }
        else
        {
            ColorMessage("Такого файла не существует: ", ConsoleColor.Yellow);
        }
        return 0;
    }
    public static int EditBoolRow(string? fileName)
    {
        IfNull("Введите название файла: ", ref fileName);
        CSVFile fileCSV = new(fileName!);
        if (File.Exists(fileCSV.File.FullPath))
        {
            int index = WriteColumn(fileCSV.File.NameFile);
            string requiredData = GetStringWriteColumn(fileCSV.GetColumn(index));
            Key($"Введите на что {requiredData} поменять(true/false): ",
                out ConsoleKey key, ConsoleKey.T, ConsoleKey.F);
            string? modifiedData = key switch
            {
                ConsoleKey.T => true.ToString(),
                ConsoleKey.F => false.ToString(),
                _ => null
            };

            if (AccessVerificationOne(fileName!, requiredData!, index))
            {
                fileCSV.File.EditingRow(
                    requiredData: requiredData!,
                    modifiedData: modifiedData!,
                    indexColumn: index,
                    indexColumnWrite: 1); // 1 в indexColumnWrite это bool строка таска
                return 1;
            }
            else
            {
                ColorMessage("Вы не можете менять статусы объектов других пользователей! ");
            }
        }
        else
        {
            ColorMessage("Такого файла не существует: ", ConsoleColor.Yellow);
        }
        return 0;
    }
    public static int FixingIndexing(string? fileName)
    {
        IfNull("Введите название файла: ", ref fileName);
        OpenFile file = new(fileName!);
        file.ReIndexFile(true);
        return 1;
    }
}