using Spectre.Console;
using static TodoList.Input;
using static TodoList.WriteToConsole;
namespace TodoList;

public partial class Commands
{
    public static bool AvailabilityOfAccessModifier(CSVFile fileCSV, string dataType = "auid") =>
        fileCSV.DataType!.Items.Contains(dataType);
    public static int IndexOfAccessModifier(CSVFile fileCSV, string dataType = "auid")
    {
        if (AvailabilityOfAccessModifier(fileCSV: fileCSV, dataType: dataType))
        {
            return Array.IndexOf(fileCSV.DataType!.Items.ToArray(), dataType);
        }
        return -1;
    }
    public static List<string> GetColumnAccessModifier(CSVFile fileCSV, string dataType = "auid")
    {
        List<string> list = new();
        if (AvailabilityOfAccessModifier(fileCSV: fileCSV, dataType: dataType))
        {
            int i = IndexOfAccessModifier(fileCSV: fileCSV, dataType: dataType);
            list = fileCSV.GetColumn(i);
        }
        return list;
    }
    public static string GetObjAccessModifier(CSVFile fileCSV, string requiredData, int index, string dataType = "auid")
    {
        string obj = "";
        if (AvailabilityOfAccessModifier(fileCSV: fileCSV, dataType: dataType))
        {
            int i = IndexOfAccessModifier(fileCSV: fileCSV, dataType: dataType);
            obj = fileCSV.File.SearchLineOnDataInLine(requiredData, index).Objects[0][i];
        }
        return obj;
    }
    public static bool AccessVerificationList(string fileName)
    {
        string dataType = "auid";
        CSVFile fileCSV = new(fileName);
        bool access = true;
        string auid = Input.GetActiveUID();
        if (AvailabilityOfAccessModifier(fileCSV, dataType))
        {
            foreach (var uid in GetColumnAccessModifier(fileCSV, dataType))
            {
                if (auid != uid)
                {
                    access = false;
                }
            }
        }
        return access;
    }
    public static bool AccessVerificationOne(string fileName, string requiredData, int index)
    {
        string dataType = "auid";
        CSVFile fileCSV = new(fileName);
        bool access = true;
        string auid = Input.GetActiveUID();
        if (AvailabilityOfAccessModifier(fileCSV, dataType))
        {
            string uid = GetObjAccessModifier(fileCSV, requiredData, index, dataType);
            if (auid != uid) { access = false; }
        }
        return access;
    }
    public static int SearchPartData(string? fileName = "", string? text = "", int indexColumn = -1)
    {
        IfNull("Ведите название файла: ", ref fileName);
        CSVFile fileCSV = new(fileName!);
        if (File.Exists(fileCSV.File.FullPath))
        {
            IfNull("Поиск: ", ref text);
            if (indexColumn == -1)
            {
                indexColumn = WriteColumn(fileCSV.File.NameFile);
            }
            CSVFile searchFileCSV = fileCSV.File.SearchLineOnDataInLine(text!, indexColumn);
            var table = new Table();
            table.Title(fileName!);
            foreach (string? titleRow in searchFileCSV.Title!.Items)
            {
                table.AddColumns(titleRow!);
            }
            foreach (var line in searchFileCSV.Objects)
            {
                table.AddRow(line.GetStringArray());
            }
            AnsiConsole.Write(table);
            return 1;
        }
        else
        {
            ColorMessage(fileName + ": такого файла не существует.", ConsoleColor.Red);
            return 0;
        }
    }
    public static CSVLine SearchActiveProfile()
    {
        List<CSVLine> activeProfile = Profile.Pattern.File.SearchLineOnDataInLine(true.ToString(), 1).Objects;
        if (activeProfile.Count != 1) //если количество активных аккаунтов больше чем 1
        {
            LogInOrCreate();
            return Profile.Pattern.File.SearchLineOnDataInLine(true.ToString(), 1).Objects[0]; //обновляем список
        }
        return activeProfile[0];
    }
}