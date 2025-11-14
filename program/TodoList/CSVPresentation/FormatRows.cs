// This file contains row and date formatting - PoneMaurice
using static TodoList.Const;
namespace TodoList;

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
	/// <param name="type">Тип строки от которого зависит стартовый набор элементов</param>
	public FormatterRows(TypeRow type)
	{
		Type = type;
		Items = GetFirstObject()!;
	}
	/// <summary>
	/// Добавляет один элемент в строку
	/// </summary>
	/// <param name="partRow">Элемент который будет добавлен</param>
	public void AddInRow(string? partRow) => Items.Add(partRow!);
}
public static class Const
{
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
	/// Название файла с тасками
	/// </summary>
	public const string TaskName = "Tasks";
	/// <summary>
	/// Название файла с профилями
	/// </summary>
	public const string ProfileName = "Profiles";
	/// <summary>
	/// Название файла с логами
	/// </summary>
	public const string LogName = "log";
	public const string PrintInTerminal = "-- ";
}