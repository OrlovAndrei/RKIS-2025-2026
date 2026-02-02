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
			"read \"номер\" - просмотреть полный текст задачи\n" +
			"status \"номер\" \"not/progress/done/postpone/fail\" - отметить статус задачи\n" +
			"delete \"номер\" - удалить задачу\n" +
			"update \"номер\" \"новый текст\" - обновить задачу\n" +
			"exit - выйти из программ\n"
			);
		}
		public override void Unexecute()
		{
			Console.WriteLine("Отмена команды помощи (нет изменений для отмены)");
		}
	}
}