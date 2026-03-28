using TodoApp.Exceptions;

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
						 "status - изменяет статус задачи (status индекс статус)\n" +
						 "delete - удаляет задачу по индексу\n" +
						 "update - обновляет текст задачи\n" +
						 "search - поиск задач (флаги: --contains/-c, --starts/-s, --ends/-e, --status/-st, --sort, --desc, --top/-t)\n" +
						 "sync - синхронизация с сервером\n" +
						 "    sync -pull - загрузить данные с сервера\n" +
						 "    sync -push - отправить данные на сервер\n" +
						 "    sync - полная синхронизация (push + pull)\n" +
						 "load - имитация параллельной загрузки (load кол-во размер)\n" +
						 "undo - отменить последнее действие\n" +
						 "redo - повторить отмененное действие\n" +
						 "exit - выйти");
	}
}