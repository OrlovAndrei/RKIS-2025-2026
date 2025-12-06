namespace TodoList;

/// <summary>
/// Паттерн содержащий в себе все нужное для работы с объектами типа title 
/// </summary>
public static class Task
{
	private static readonly CSVLine title = new("Counter", "Bool", "Task Name", "Description", "Creation date", "DeadLine");
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
public static class Profile
{
	private static readonly CSVLine title = new("UID", "Active", "Login", "First Name", "Last Name", "Creation date", "Birth");
	private static readonly CSVLine dataType = new("ruid", "false", "s", "s", "s", "ndt", "d");
	private static readonly string FileName = "Profiles";
	public static readonly CSVFile Pattern = new(FileName, title, dataType);
}
public static class Password
{
	private static readonly CSVLine title = new("UID", "Password");
	private static readonly CSVLine dataType = new("uid", "pas");
	private static readonly string FileName = "Password";
	public static readonly CSVFile Pattern = new(FileName, title, dataType);
	public static string? GetUIDWithoutPassword()
	{
		List<string> resultList = new();
		List<string> ruid = Profile.Pattern.GetColumn(0); //UID в профилях
		if (File.Exists(Password.Pattern.File.FullPath))
		{
			List<string> uid = Password.Pattern.GetColumn(0); //UID в паролях
			foreach (var ruidString in ruid)
			{
				if (!uid.Contains(ruidString))
				{
					resultList.Add(ruidString);
				}
			}
		}
		else
		{
			resultList = ruid;
		}
		string? result = null;
		if (resultList.Count() > 0)
		{
			result = resultList[^1];
		}
		return result;
	}
}
/// <summary>
/// Паттерн содержащий в себе все нужное для работы с объектами типа log 
/// </summary>
public static class Log
{
	private static readonly CSVLine title = new("Counter", "Status", "Active Profile", "Date", "Command", "Options", "Text Command");
	private static readonly CSVLine dataType = new("counter", "lb", "prof", "ndt", "command", "option", "textline");
	private static readonly string FileName = "Log";
	public static readonly CSVFile Pattern = new(FileName, title, dataType);
}