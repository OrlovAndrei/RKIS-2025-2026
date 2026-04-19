namespace TodoList.Commands;

public class HelpCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine(
			"Команды:\n" +
			"help — выводит список всех доступных команд с кратким описанием\n" +
			"profile — выводит данные пользователя\n" +
			"add \"текст задачи\" — добавляет новую задачу\n" +
			"    -m, --multi — добавить многострочную задачу\n" +
			"view — выводит все задачи\n" +
			"    -a, --all — показать все поля\n" +
			"    -i, --index — показать индекс\n" +
			"    -s, --status — показать статус\n" +
			"    -d, --update-date — показать дату\n" +
			"read \"idx\" — вывод задачи без редактирования\n" +
			"done \"idx\" — отметить задачу выполненной\n" +
			"delete \"idx\" — удалить задачу\n" +
			"update \"idx\" — изменить текст задачи\n" +
			"exit — выход из программы"
		);
	}
}