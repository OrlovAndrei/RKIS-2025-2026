namespace TodoApp.Commands
{
	public class HelpCommand : BaseCommand
	{
		public override void Execute()
		{
			Console.Write(
			"Доступные команды:\n" +
			"help - показать все команды\n" +
			"profile - показать профиль\n" +
			"profile --out - сохранить профиль и выйти из него\n" +
			"add \"задача\" - добавить задачу\n" +
			"add --multiline / add -m - многострочный режим добавления\n" +
			"view - показать все задачи\n" +
			"view --index / view -i - показать задачи с индексами\n" +
			"view --status / view -s - показать задачи со статусом\n" +
			"view --date / view -d - показать задачи с датой изменения\n" +
			"view --all или view -a - показать все данные задач\n" +
			"search - поиск задач с фильтрами\n" +
            "search --contains \"текст\" - задачи, содержащие текст\n" +
            "search --starts-with \"текст\" - задачи, начинающиеся с текста\n" +
            "search --ends-with \"текст\" - задачи, заканчивающиеся текстом\n" +
            "search --from yyyy-MM-dd - задачи с даты\n" +
            "search --to yyyy-MM-dd - задачи до даты\n" +
            "search --status <статус> - фильтр по статусу\n" +
            "search --sort text|date - сортировка\n" +
            "search --desc - сортировка по убыванию\n" +
        	"search --top N - ограничить количество результатов\n" +
			"read \"номер\" - просмотреть полный текст задачи\n" +
			"status \"номер\" \"not/progress/done/postpone/fail\" - отметить статус задачи\n" +
			"delete \"номер\" - удалить задачу\n" +
			"update \"номер\" \"новый текст\" - обновить задачу\n" +
			"undo - отменить последнюю команду\n" +
			"redo - повторить отмененную команду\n" +
			"exit - выйти из программ\n" +
			"load <количество_скачиваний> <размер_скачиваний> - запустить параллельные загрузки с прогресс‑барами\n"
			);
		}
	}
}