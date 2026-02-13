using System;

namespace TodoList
{
	class HelpCommand : ICommand
	{
		public void Execute()
		{
			Console.WriteLine(@"Доступные команды:
        help - справка по командам
        profile - данные пользователя
        add - добавить задачу (--multiline/-m, --urgent/-u)
        view - список задач (--index/-i, --status/-s, --update-date/-d, --all/-a)
        read - полный текст задачи
	status - изменить статус задачи (not_started, in_progress, completed, postponed, failed)
	search - поиск задач (--contains, --starts-with, --ends-with, --from, --to, --status, --sort, --desc, --top)
        delete - удалить задачу
	undo - отменить последнее действие
	redo - повторить отмененное действие
        update - изменить текст задачи
        exit - выход из программы");
		}
	}
}