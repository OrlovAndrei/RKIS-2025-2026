using System.Text;
using Spectre.Console;
using static TodoList.Input;
using static TodoList.WriteToConsole;
namespace TodoList;

public partial class Commands
{
    public static int Print(CSVLine row, CSVLine title)
	{
		var table = new Table();
		if (title.GetLength() != 0 && row.GetLength() != 0)
		{
			table.AddColumns(title[0]!);
			table.AddColumns(row[0]!);
			for (int i = 1; i < title.GetLength(); i++)
			{
				table.AddRow(title[i]!, row[i]!);
			}
		}
		AnsiConsole.Write(table);
		return 1;
	}
	public static int PrintActivePriFile()
	{
		Print(SearchActiveProfile(), Profile.Pattern.Title!);
		return 1;
	}
    public static int PrintAll(string? fileName = "")
    {
        IfNull("Ведите название файла: ", ref fileName);
        CSVFile fileCSV = new(fileName!);
        try
        {
            using (StreamReader reader = new StreamReader(fileCSV.File.FullPath, Encoding.UTF8))
            {
                CSVLine line;
                var table = new Table();
                table.Title(fileName!);
                foreach (string? titleRow in fileCSV.Title!.Items)
                {
                    table.AddColumns(titleRow!);
                }
                while ((line = new(reader.ReadLine())).GetLength() != 0)
                {
                    table.AddRow(line.GetStringArray());
                }
                AnsiConsole.Write(table);
                return 1;
            }
        }
        catch (Exception)
        {
            RainbowText("Произошла ошибка при чтении файла", ConsoleColor.Red);
            return 0;
        }
    }
    public static int WriteCaption()
	{
		/*спрашивает и выводит текст субтитров созданный 
	        методом CompText*/
		Text(
			"За работу ответственны:",
			"\tШевченко Э. - README, исходный код;",
			"\tТитов М. - github, некоторый аспекты исходного кода, help команды;"
		);
		return 1;
	}
}