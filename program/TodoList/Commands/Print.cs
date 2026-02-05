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
		if (title.Length() != 0 && row.Length() != 0)
		{
			table.AddColumns(title[0]!);
			table.AddColumns(row[0]!);
			for (int i = 1; i < title.Length(); i++)
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
		using (StreamReader reader = new StreamReader(fileCSV.File.FullPath, Encoding.UTF8))
		{
			CSVLine line;
			var table = new Table();
			table.Title(fileName!);
			foreach (string? titleRow in fileCSV.Title!.Items)
			{
				table.AddColumns(titleRow!);
			}
			while ((line = new(reader.ReadLine())).Length() != 0)
			{
				table.AddRow(line.GetStringArray());
			}
			AnsiConsole.Write(table);
		}
		return 1;
	}
	public static int PrintSpecific(string[] columnName, string? fileName = "")
	{
		IfNull("Ведите название файла: ", ref fileName);
		CSVFile fileCSV = new(fileName!);
		using (StreamReader reader = new StreamReader(fileCSV.File.FullPath, Encoding.UTF8))
		{
			CSVLine line;
			var table = new Table();
			table.Title(fileName!);
			int columnId = 0;
			List<int> neededColumnsId = new List<int>();
			foreach (string? titleRow in fileCSV.Title!.Items)
			{
				columnId++;
				if (columnName.Contains(titleRow))
				{
					neededColumnsId.Add(columnId);
					table.AddColumns(titleRow!);
				}
			}
			int rowId = 0;
			List<string> stringRowList = new List<string>();
			while ((line = new(reader.ReadLine())).Length() != 0)
			{
				rowId = 0;
				stringRowList.Clear();
				foreach (string? row in line.Items)
				{
					rowId++;
					if (neededColumnsId.Contains(rowId))
					{
						stringRowList.Add(row!);
					}
				}
				table.AddRow(stringRowList.ToArray());
			}
			AnsiConsole.Write(table);
		}
		return 1;
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