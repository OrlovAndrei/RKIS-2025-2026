using CommandLine;

namespace ShevricTodo.Parser.Verb;


[Verb(name: "profile", isDefault: false, HelpText = "Работа с профилями.")]
internal class Profile
{
	[Option(longName: "add", shortName: 'a', Default = false, HelpText = "Добавить профиль.", Group = "add")]
	public bool Add { get; set; }
	[Option(longName: "list", shortName: 'l', Default = false, HelpText = "Просмотреть список профилей.", Group = "list")]
	public bool List { get; set; }
	[Option(longName: "search", shortName: 's', Default = false, HelpText = "Найти профиль.", Group = "search")]
	public bool Search { get; set; }
	[Option(longName: "remove", shortName: 'r', Default = false, HelpText = "Удалить профиль.", Group = "remove")]
	public bool Remove { get; set; }
}

