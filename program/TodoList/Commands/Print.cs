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
	public static int PrintSpecific(string columnName, string? fileName = "")
	{
		Console.WriteLine("hui1");
		IfNull("Ведите название файла: ", ref fileName);
		Console.WriteLine("hui2");
		OpenFile file = new(fileName!);
		Console.WriteLine(fileName);
		try
		{
			using (StreamReader reader = new StreamReader(file.fullPath, Encoding.UTF8))
			{
				string? line;
				Console.WriteLine("hui3");
				string[] titleRowArray = (reader.ReadLine() ?? "").Split(SeparRows);				
				foreach (string number in titleRowArray)
				{
    				Console.WriteLine(number);
				}
				Console.WriteLine("hui4");
				var table = new Table();
				Console.WriteLine("hui5");
				table.Title(fileName!);
				Console.WriteLine("hui6");
				Console.WriteLine("hui7");
				int columnId = 0;
				List<int> neededColumnsId = new List<int>();
				foreach (string titleRow in titleRowArray)
				{
					if (titleRow == columnName || titleRow == "numbering" || titleRow == "nameTask" || (columnName == "deadLine" && titleRow == "nowDateAndTime"))
					{
						Console.WriteLine("hui");
						Console.WriteLine(titleRow);
						columnId++;
						neededColumnsId.Add(columnId);
						table.AddColumns(titleRow);
					}
					else
					{
						Console.WriteLine("huiNOOOOOOOOOO");
						Console.WriteLine(titleRow);
						columnId++;
						continue;
					}
				}
				string stringRow = "";
				int rowId = 0;
				StringBuilder stringRowBuilder = new StringBuilder();
				while ((line = reader.ReadLine()) != null)
				{
					stringRow = "";
					rowId = 0;
					stringRowBuilder.Clear();
					foreach (string row in line.Split(SeparRows))
					{
						rowId++;
						if (neededColumnsId.Contains(rowId))
						{
							stringRowBuilder.Append(row + "|");
							Console.WriteLine(row);
						}
					}
					stringRowBuilder.Length--;
					stringRow = stringRowBuilder.ToString();
					Console.WriteLine(stringRow);
					foreach (string number in stringRow.Split("|"))
					{
						Console.WriteLine(number);
					}
					table.AddRow(stringRow.Split("|"));
				}
				AnsiConsole.Write(table);
				return 1;
			}
		}
		catch (Exception e)
		{
			RainbowText($"Произошла ошибка при чтении файла {e.Message}", ConsoleColor.Red);
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