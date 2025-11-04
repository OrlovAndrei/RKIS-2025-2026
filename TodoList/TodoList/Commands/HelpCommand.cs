public class HelpCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine("help - выводит список всех доступных команд\n" +
						 "profile - выводит ваши данные\n" +
						 "add - добавляет новую задачу (add \"Новая задача\")(флаги: add --multiline/-m, !end)\n" +
						 "view - просмотр задач (флаги: --index/-i, --status/-s, --update-date/-d, --all/-a)\n" +
						 "read idx - просмотр полного текста задач\n" +
						 "done - отмечает задачу выполненной\n" +
						 "delete - удаляет задачу по индексу\n" +
						 "update\"new_text\"- обновляет текст задачи\n" +
						 "exit - выйти");
	}
}
