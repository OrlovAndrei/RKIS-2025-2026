// This file contains row and date formatting - PoneMaurice
using static Task.Const;
namespace Task;

/// <summary>
/// title - пригоден для титульного оформления,
/// row - пригоден для стандартных строк,
/// dataType - пригоден для типов данных,
/// old - пригоден для форматирования уже существующих строк
/// </summary>
public enum TypeRow
{
	title,
	dataType
}

public class FormatterRows : CSVLine
{
	/// <summary>
	/// Номер строки
	/// </summary>
	/// <value></value>
	public int Num { set; private get; }
	private TypeRow Type { set; get; }
	/// <summary>
	/// В зависимости от переданного при инициализации типа строки
	/// возвращает стартовые элементы строки
	/// </summary>
	/// <returns>Список стартовых строк</returns>
	public List<string> GetFirstObject()
	{
		List<string> res = Type switch
		{
			TypeRow.title =>
				[TitleNumbingObject, TitleBoolObject],
			TypeRow.dataType =>
				[DataTypeNumbingObject, "false"],
			_ => new()
		};
		return res;
	}
	/// <summary>
	/// Вычисляет номер последней строки, добавляет тип строки
	/// и формирует базовый вариант строки на основе типа
	/// </summary>
	/// <param name="nameFile">Имя файла в который будет записываися строка</param>
	/// <param name="type">Тип строки от которого зависит стартовый набор элементов</param>
	public FormatterRows(string nameFile, TypeRow type)
	{
		OpenFile file = new(nameFile);
		Num = file.GetLengthFile();
		Type = type;
		Items = GetFirstObject()!;
	}
	/// <summary>
	/// Добавляет один элемент в строку
	/// </summary>
	/// <param name="partRow">Элемент который будет добавлен</param>
	public void AddInRow(string? partRow) => Items.Add(partRow!);
	/// <summary>
	/// Добавляет несколько элементов в сроку
	/// </summary>
	/// <param name="row">Массив который будет добавлен в строку</param>
	public void AddInRow(string[] row)
	{
		foreach (var partRow in row)
		{
			AddInRow(partRow);
		}
	}
	
	/// <summary>
	/// Форматирует строку перед выводом
	/// </summary>
	/// <returns>Форматированная строка</returns>
}
public static class Const
{
	/// <summary>
	/// Разделитель которым происходит форматирование строк
	/// </summary>
	public const string SeparRows = "|";
	/// <summary>
	/// Объект для оформления титульной строки
	/// </summary>
	public const string TitleNumbingObject = "Numbering";
	/// <summary>
	/// Особый тип данных для файла конфигурации
	/// </summary>
	public const string DataTypeNumbingObject = "counter";
	/// <summary>
	/// Объект титульного оформления
	/// </summary>
	public const string TitleBoolObject = "Bool";
	/// <summary>
	/// Особый тип данных для файла конфигурации
	/// </summary>
	public const string DataTypeBoolObject = "bool";
	/// <summary>
	/// Стандартное значение колоны состояния строки  
	/// </summary>
	public const string RowBoolDefault = "False";
	/// <summary>
	/// Окончание для файла конфигурации
	/// </summary>
	public const string PrefConfigFile = "_conf";
	/// <summary>
	/// Окончание для временного файла
	/// </summary>
	public const string PrefTemporaryFile = "_temp";
	/// <summary>
	/// Окончание файла для работы с индексами
	/// </summary>
	public const string PrefIndex = "_index";
	/// <summary>
	/// Название файла с тасками
	/// </summary>
	public const string TaskName = "Tasks";
	/// <summary>
	/// Название файла с профилями
	/// </summary>
	public const string ProfileName = "Profiles";
	/// <summary>
	/// Титульное оформление для файла с тасками
	/// </summary>
	/// <value></value>
	public static readonly string[] TaskTitle = { "nameTask", "description", "nowDateAndTime", "deadLine" };
	/// <summary>
	/// Типы данных для колонок таскав
	/// </summary>
	/// <value></value>
	public static readonly string[] TaskTypeData = { "s", "ls", "ndt", "dt" };
	/// <summary>
	/// Титульное оформление для файла с профилями
	/// </summary>
	/// <value></value>
	public static readonly string[] ProfileTitle = { "name", "DOB", "nowDateAndTime" };
	/// <summary>
	/// Типы данных для колонок профилей
	/// </summary>
	/// <value></value>
	public static readonly string[] ProfileDataType = { "s", "d", "ndt" };
	/// <summary>
	/// "Нулевой" пользователь
	/// </summary>
	/// <returns></returns>
	public static readonly string[] AdminProfile = { "guest", "None", Input.NowDateTime() };
	/// <summary>
	/// Название файла с логами
	/// </summary>
	public const string LogName = "log";
	/// <summary>
	/// Титульное оформление для файла с логами
	/// </summary>
	/// <value></value>
	public static readonly string[] LogTitle = { "ActiveProfile", "DateAndTime", "Command", "Options", "TextCommand" };
	/// <summary>
	/// Типы данных для колонок логов
	/// </summary>
	/// <value></value>
	public static readonly string[] LogDataType = { "prof", "ndt", "command", "option", "textline" };
	public const string PrintInTerminal = "-- ";
	/// <summary>
	/// Название директории с данными связанными с приложением  
	/// </summary>
	public const string DirectoryName = "RKIS-TodoList";
}