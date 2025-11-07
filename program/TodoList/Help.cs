using static Task.WriteToConsole;
namespace Task;

public class Helpers
{
	public static int ProfileHelp()
	{
		Text(
			"- `profile --help` — помощь",
			"- `profile --add` — добавить профиль",
			"- `profile --change` — сменить активный профиль",
			"- `profile --index` — переиндексация профилей",
			"- `profile` — показать активный профиль"
		);
		return 1;
	}
	public static int Help()
	{
		Text(
			"- `add` — добавление данных, задач или профилей",
			"- `profile` — работа с профилями",
			"- `print` — вывод информации",
			"- `search` — поиск по данным",
			"- `clear` — очистка данных",
			"- `edit` — редактирование данных",
			"- `help` — выводит общую справку по всем командам",
			"- `exit` — завершает выполнение программы"
		);
		return 1;
	}
	public static int AddHelp()
	{
		Text(
			"- `add --help` — помощь по добавлению",
			"- `add --task` — добавить новую задачу",
			"- `add --multi --task` — добавить несколько задач сразу",
			"- `add --task --print` — добавить задачу и сразу вывести её",
			"- `add --config <имя>` — добавить конфигурацию",
			"- `add --profile` — добавить профиль",
			"- `add <текст>` — добавить пользовательские данные"
		);
		return 1;
	}
	public static int PrintHelp()
	{
		Text(
			"- `print --help` — помощь",
			"- `print --task` — вывести все задачи",
			"- `print --config <имя>` — вывести конфигурацию",
			"- `print --profile` — вывести профили",
			"- `print --captions` — вывести заголовки",
			"- `print <имя>` — вывести данные по имени"
		);
		return 1;
	}
	public static int SearchHelp()
	{
		Text(
			"- `search --help` — помощь",
			"- `search --task <текст>` — поиск по задачам",
			"- `search --profile <текст>` — поиск по профилям",
			"- `search --numbering` — (в разработке)",
			"- `search <текст>` — общий поиск"
		);
		return 1;
	}
	public static int ClearHelp()
	{
		Text(
			"- `clear --help` — помощь",
			"- `clear --task <имя>` — удалить задачу",
			"- `clear --task --all` — очистить все задачи",
			"- `clear --profile <имя>` — удалить профиль",
			"- `clear --profile --all` — очистить все профили",
			"- `clear --console` — очистить консоль",
			"- `clear --all <текст>` — очистить все пользовательские данные"
		);
		return 1;
	}
	public static int EditHelp()
	{
		Text(
		   "- `edit --help` — помощь",
			"- `edit --task <имя>` — изменить задачу",
			"- `edit --task --index` — переиндексация задач",
			"- `edit --task --bool` — изменить главное логическое поле задачи",
			"- `edit --bool` — изменить главное логическое поле в данных",
			"- `edit --index` — переиндексация",
			"- `edit <имя>` — редактировать по имени"
		);
		return 1;
	}
}