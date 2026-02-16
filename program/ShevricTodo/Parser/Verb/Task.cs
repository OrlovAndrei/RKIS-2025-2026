using CommandLine;

namespace ShevricTodo.Parser.Verb;

[Verb(name: "task", isDefault: false, HelpText = "Работа с задачами.")]
internal class Task
{
	[Option(longName: "add", shortName: 'a', Default = false, HelpText = "Добавить задачу.", Group = "add")]
	public bool Add { get; set; }
	[Option(longName: "name", shortName: 'n', Default = null, HelpText = "Добавить задачу c именем.", Group = "add")]
	public string? Name { get; set; }
	[Option(longName: "description", shortName: 'd', Default = null, HelpText = "Добавить задачу c описанием.", Group = "add")]
	public string? Description { get; set; }
	[Option(longName: "date", shortName: 'D', Default = null, HelpText = "Добавить задачу c дедлайном.", Group = "add")]
	public string? Deadline { get; set; }
	[Option(longName: "start", shortName: 'S', Default = false, HelpText = ".", Group = "search")]
	public bool StartWith { get; set; }
	[Option(longName: "ends", shortName: 'E', Default = false, HelpText = ".", Group = "search")]
	public bool EndsWith { get; set; }
	[Option(longName: "list", shortName: 'l', Default = false, HelpText = "Просмотреть список задач.", Group = "list")]
	public bool List { get; set; }
	[Option(longName: "search", shortName: 's', Default = false, HelpText = "Найти задачу.", Group = "search")]
	public bool Search { get; set; }
	[Option(longName: "remove", shortName: 'r', Default = false, HelpText = "Удалить задачу.", Group = "remove")]
	public bool Remove { get; set; }
}
