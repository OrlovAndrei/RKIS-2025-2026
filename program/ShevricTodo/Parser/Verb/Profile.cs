using CommandLine;

namespace ShevricTodo.Parser.Verb;


[Verb(name: "profile", isDefault: false, HelpText = "Работа с профилями.")]
internal class Profile
{
	[Option(longName: "add", shortName: 'a', Default = false, HelpText = "Добавить профиль.", Group = "add")]
	public bool Add { get; set; }
	[Option(longName: "First-Name", shortName: 'F', Default = null, HelpText = "Добавить профиль с именем", Group = "add")]
	public string? FirstName { get; set; }
	[Option(longName: "Last-Name", shortName: 'L', Default = null, HelpText = "Добавить профиль с фамилией", Group = "add")]
	public string? LastName { get; set; }
	[Option(longName: "Nickname", shortName: 'N', Default = null, HelpText = "Добавить профиль с никнеймом", Group = "add")]
	public string? UserName { get; set; }
	[Option(longName: "Birth", shortName: 'B', Default = null, HelpText = "Добавить профиль с днём рождения", Group = "add")]
	public string? Birthday { get; set; }
	[Option(longName: "start", shortName: 'S', Default = false, HelpText = ".", Group = "search")]
	public bool StartWith { get; set; }
	[Option(longName: "ends", shortName: 'E', Default = false, HelpText = ".", Group = "search")]
	public bool EndsWith { get; set; }
	[Option(longName: "list", shortName: 'l', Default = false, HelpText = "Просмотреть список профилей.", Group = "list")]
	public bool List { get; set; }
	[Option(longName: "search", shortName: 's', Default = false, HelpText = "Найти профиль.", Group = "search")]
	public bool Search { get; set; }
	[Option(longName: "remove", shortName: 'r', Default = false, HelpText = "Удалить профиль.", Group = "remove")]
	public bool Remove { get; set; }
	[Option(longName: "change", shortName: 'c', Default = false, HelpText = "Сменить профиль.", Group = "change")]
	public bool Change { get; set; }
}
