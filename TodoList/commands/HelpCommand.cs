namespace TodoList.commands;

public class HelpCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine("help - выводит список всех доступных команд\n" +
		                  "profile - выводит ваши данные\n" +
		                  "add text (--multiline/-m) - добавляет новую задачу\n" +
		                  "view (--all/-a, --index/-i, --status/-s, --update-date/-d) - просмотр задач\n" +
		                  "read idx - просмотр полного текста задач\n" +
		                  "done idx - отмечает задачу выполненной\n" +
		                  "delete idx - удаляет задачу по индексу\n" +
		                  "update idx text - обновляет текст задачи");
	}
}