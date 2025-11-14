using static TodoList.Input;
using static TodoList.WriteToConsole;
namespace TodoList;

public partial class Commands
{
    public static int AddTask()
    {
        /*программа запрашивает у пользователя все необходимые ей данные
            и записывает их в файл tasks.csv с нужным форматированием*/
        OpenFile file = Task.Pattern.File;
        file.AddRowInFile(Task.Pattern);
        return 1;
    }
    public static int MultiAddTask()
    {
        int num = 0;
        while (true)
        {
            AddTask();
            num++;
            if (!Bool($"{num} задание добавлено, желаете продолжить?"))
            {
                break;
            }
        }
        return 1;
    }
    public static int AddTaskAndPrint()
    {
        /*программа запрашивает у пользователя все необходимые ей данные
	        и записывает их в файл tasks.csv с нужным форматированием 
	        после чего выводит сообщение о добавлении данных дублируя их 
	        пользователю для проверки*/
        OpenFile file = Task.Pattern.File;
        file.AddRowInFile(Task.Pattern);
        Print(file.GetLinePositionRow(file.GetLengthFile() - 1), file.GetLinePositionRow(0));
        return 1;
    }
    public static int AddConfUserData(string? fileName = "")
    {
        IfNull("Введите название для файла с данными: ", ref fileName);
        CSVFile fileCSV = new(fileName!);
        CSVLine lastTitleRow = new(), lastDataTypeRow = new();
        bool askFile = true;
        if (File.Exists(fileCSV.ConfigFile.FullPath))
        {
            lastTitleRow = fileCSV.Title!;
            lastDataTypeRow = fileCSV.DataType!;
            Print(lastDataTypeRow, lastTitleRow);
            askFile = Bool($"Вы точно уверены, что хотите перезаписать конфигурацию?");
        }
        if (askFile)
        {
            FormatterRows titleRow = new(TypeRow.title),
            dataTypeRow = new(TypeRow.dataType);
            while (true)
            {
                string intermediateResultString =
                    String("Введите название пункта титульного оформления файла: ");
                if (intermediateResultString == "exit" &&
                titleRow.GetLength() != 0) break;
                else if (intermediateResultString == "exit")
                    RainbowText("В титульном оформлении должен быть хотя бы один пункт: ", ConsoleColor.Red);
                else if (titleRow.Items!.Contains(intermediateResultString))
                {
                    RainbowText("Объекты титульного оформления не должны повторятся", ConsoleColor.Red);
                }
                else titleRow.AddInRow(intermediateResultString);
            }
            foreach (string? title in titleRow.Items!)
            {
                if (titleRow.GetFirstObject().Contains(title!)) continue;
                else dataTypeRow.AddInRow(DataType($"Введите тип данных для строки {title}: "));
            }
            OpenFile.AddFirst(fileCSV);
            bool ask = true;
            if ((lastTitleRow.Items != titleRow.Items && lastTitleRow.GetLength() != 0) ||
            (lastDataTypeRow.Items != dataTypeRow.Items && lastDataTypeRow.GetLength() != 0))
            {
                Console.WriteLine("Нынешний: ");
                Print(dataTypeRow, titleRow);
                Console.WriteLine("Прошлый: ");
                Print(lastDataTypeRow, lastTitleRow);
                ask = Bool("Заменить?: ");
            }
            else
            {
                Print(dataTypeRow, titleRow);
            }
            if (ask)
            {
                fileCSV.DataType = dataTypeRow;
                fileCSV.Title = titleRow;
                OpenFile.AddFirst(fileCSV, true);
            }
            return 1;
        }
        else
        {
            RainbowText("Будет использована конфигурация: ", ConsoleColor.Yellow);
            Print(fileCSV.DataType!, fileCSV.Title!);
            return 0;
        }
    }
    public static int AddUserData(string? fileName = "")
    {
        IfNull("Введите название для файла с данными: ", ref fileName);
        CSVFile fileCSV = new(fileName!);
        if (File.Exists(fileCSV.ConfigFile.FullPath))
        {
            RowOnTitleAndConfig(fileCSV, out CSVLine outLine);
            fileCSV.File.WriteFile(outLine);
            return 1;
        }
        else
        {
            RainbowText($"Сначала создайте конфигурацию или проверьте правильность написания названия => '{fileName}'", ConsoleColor.Red);
            return 0;
        }
    }
    public static int AddLog()
    {
        try
        {
            if (Survey.CommandLineGlobal != null)
            {
                Log.Pattern.File.AddRowInFile(Log.Pattern, false);
                return 1;
            }
        }
        catch (Exception)
        {
            RainbowText("Произошла ошибка при записи файла", ConsoleColor.Red);
            return 0;
        }
        return 0;
    }
    public static int AddProfile()
	{
		OpenFile profileFile = Profile.Pattern.File;
		profileFile.AddRowInFile(Profile.Pattern);
		return 1;
	}
    public static int AddFirstProfile()
    {
        OpenFile.AddFirst(Profile.Pattern);
        OpenFile profile = Profile.Pattern.File;
        if (profile.GetLengthFile() == 1)
        {
            AddProfile();
            profile.EditingRow(false.ToString(), true.ToString(), 1);
            return 1;
        }
        return 0;
    }
    public static int UseActiveProfile()
    {
        if (File.Exists(Profile.Pattern.File.FullPath))
        {
            Profile.Pattern.File.EditingRow(true.ToString(), false.ToString(), 1, -1);
            string requiredData = Input.String("Поиск: ");
            string modifiedData = true.ToString();
            Profile.Pattern.File.EditingRow(requiredData, modifiedData,
            WriteColumn(Profile.Pattern.File.NameFile), indexColumnWrite: 1); // 1 в indexColumnWrite это bool строка таска
            SearchActiveProfile();
            return 1;
        }
        else
        {
            AddFirstProfile();
            return 1;
        }
    }
}