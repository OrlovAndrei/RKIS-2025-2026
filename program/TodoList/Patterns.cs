namespace TodoList;

/// <summary>
/// Паттерн содержащий в себе все нужное для работы с объектами типа title 
/// </summary>
public class Task
{
	private static readonly CSVLine title = new("Numbering", "Bool", "Task Name", "Description", "Creation date", "DeadLine");
	private static readonly CSVLine dataType = new("counter", "status", "s", "ls", "ndt", "dt");
	private static readonly string FileName = "Tasks";
	public static readonly List<string> Status =
	["In process", "Done", "Almost completed",
	"Abandoned", "Deferred", "Failed"];
	public static readonly CSVFile Pattern = new(FileName, title, dataType);
}
/// <summary>
/// Паттерн содержащий в себе все нужное для работы с объектами типа профиля 
/// </summary>
public class Profile
{
	private static readonly CSVLine title = new("Numbering", "Bool", "Profile Name", "Creation date", "Birth");
	private static readonly CSVLine dataType = new("counter", "false", "s", "ndt", "d");
	private static readonly string FileName = "Profiles";
	public static readonly CSVFile Pattern = new(FileName, title, dataType);
}
/// <summary>
/// Паттерн содержащий в себе все нужное для работы с объектами типа log 
/// </summary>
public class Log
{
    private static readonly CSVLine title = new("Numbering", "Bool", "ActiveProfile", "Date And Time", "Command", "Options", "TextCommand");
	private static readonly CSVLine dataType = new("counter", "lb", "prof", "ndt", "command", "option", "textline");
	private static readonly string FileName = "Log";
	public static readonly CSVFile Pattern = new(FileName, title, dataType);
}