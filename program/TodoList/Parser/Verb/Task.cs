using CommandLine;

namespace ShevricTodo.Parser.Verb;

[Verb(name: "task", isDefault: true, HelpText = "Работа с задачами.")]
internal class Task
{
	[Option(longName: "add", shortName: 'a', Default = false, HelpText = "Добавить задачу.", Group = "add")]
	public bool Add { get; set; }
	[Option(longName: "list", shortName: 'l', Default = false, HelpText = "Просмотреть список задач.", Group = "list")]
	public bool List { get; set; }
	[Option(longName: "search", shortName: 's', Default = false, HelpText = "Найти задачу.", Group = "search")]
	public bool Search { get; set; }
	[Option(longName: "remove", shortName: 'r', Default = false, HelpText = "Удалить задачу.", Group = "remove")]
	public bool Remove { get; set; }
}
