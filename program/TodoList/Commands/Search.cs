using Spectre.Console;
using static TodoList.Input;
using static TodoList.WriteToConsole;
namespace TodoList;

public partial class Commands
{
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
            CSVFile searchFileCSV = fileCSV.File.GetLinePositionInRow(text!, indexColumn);
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
            RainbowText(fileName + ": такого файла не существует.", ConsoleColor.Red);
            return 0;
        }
    }
    public static CSVLine SearchActiveProfile()
	{
		List<CSVLine> activeProfile = Profile.Pattern.File.GetLinePositionInRow(true.ToString(), 1).Objects;
		if (activeProfile.Count != 1) //если количество активных аккаунтов больше чем 1
		{
			UseActiveProfile();
			return Profile.Pattern.File.GetLinePositionInRow(true.ToString(), 1).Objects[0]; //обновляем список
		}
		return activeProfile[0];
	}
}