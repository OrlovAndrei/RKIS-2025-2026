// This file contains row and date formatting - PoneMaurice
using static Task.Const;
namespace Task;

public class FormatterRows
{
	public List<string>? Row { set; get; }
	public int Num { set; private get; }
	private TypeEnum Type { set; get; }
	public enum TypeEnum
	{
		title,
		row,
		dataType,
		old
	}
	public List<string> GetFirstObject()
	{
		List<string> res = Type switch
		{
			TypeEnum.row =>
				[Num.ToString(), RowBoolDefault],
			TypeEnum.title =>
				[TitleNumbingObject, TitleBoolObject],
			TypeEnum.dataType =>
				[DataTypeNumbingObject, DataTypeBoolObject],
			TypeEnum.old => new(),
			_ => new()
		};
		return res;
	}
	public FormatterRows(string nameFile, TypeEnum type = TypeEnum.row)
	{
		OpenFile file = new(nameFile);
		Num = file.GetLengthFile();
		Type = type;
		Row = GetFirstObject();
	}
	public void AddInRow(string? partRow) =>
		Row!.Add(partRow!);
	public void AddInRow(string[] row)
	{
		foreach (var partRow in row)
		{
			AddInRow(partRow);
		}
	}
	public int GetLengthRow()
	{
		return Row!.Count;
	}
	public string GetRow()
	{
		return string.Join(SeparRows, Row!);
	}
	public void BoolIsTrue()
    {
        if (Type == TypeEnum.row && Row is not null)
        {
			Row[1] = true.ToString();
        }
    }
}
public static class Const
{
	public const string SeparRows = "|";
	public const string TitleNumbingObject = "numbering";
	public const string DataTypeNumbingObject = "counter";
	public const string TitleBoolObject = "Bool";
	public const string DataTypeBoolObject = "bool";
	public const string RowBoolDefault = "False";
	public const string PrefConfigFile = "_conf";
	public const string PrefTemporaryFile = "_temp";
	public const string PrefIndex = "_index";
	public readonly static string[] StringArrayNull = new string[0];
	public const string TaskName = "Tasks";
	public const string ProfileName = "Profiles";
	public static readonly string[] TaskTitle = { "nameTask", "description", "nowDateAndTime", "deadLine" };
	public static readonly string[] TaskTypeData = { "s", "ls", "ndt", "dt" };
	public static readonly string[] ProfileTitle = { "name", "DOB", "nowDateAndTime" };
	public static readonly string[] ProfileDataType = { "s", "d", "ndt" };
	public static readonly string[] AdminProfile = { "guest", "None", Input.NowDateTime() };
	public const string LogName = "log";
	public static readonly string[] LogTitle = { "ActiveProfile", "DateAndTime", "Command", "Options", "TextCommand" };
	public static readonly string[] LogDataType = { "prof", "ndt", "command", "option", "textline" };
	public const string PrintInTerminal = "-- ";
	public const string DirectoryName = "RKIS-TodoList";
}