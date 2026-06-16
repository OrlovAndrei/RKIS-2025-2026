namespace TodoList.commands;

public class HelpCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine("help - выводит список всех доступных команд\n" +
		                  "profile - выводит ваши данные\n" +
		                  "add \"Новая задача\" - (флаги: --multiline/-m)\n" +
		                  "view - просмотр задач (флаги: --index/-i, --status/-s, --update-date/-d, --all/-a)\n" +
		                  "read idx - просмотр полного текста задач\n" +
		                  "done idx - отмечает задачу выполненной\n" +
		                  "delete idx - удаляет задачу по индексу\n" +
		                  "update idx \"Новая задача\" - обновляет текст задачи\n" +
		                  "exit - выйти");
	}
}