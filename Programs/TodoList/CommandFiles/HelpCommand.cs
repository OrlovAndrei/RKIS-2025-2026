using System;

namespace Todolist
{
	public class HelpCommand : ICommand
	{
		TodoList ICommand.TodoList { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public void Execute()
		{
			var helpText = """
                Доступные команды
                help - вывести список команд
                profile - показать данные пользователя
                add - добавить задачу (однострочный режим)
                   --multiline или -m - добавить задачу (многострочный режим)
                view - показать только текст задачи
                   --index или -i - показать с индексами
                   --status или -s - показать со статусами
                   --update-date или -d - показать дату последнего изменения
                   --all или -a - показать все данные
                read <номер> - просмотреть полный текст задачи
                status <номер> <статус> - изменить статус задачи
                      Примеры статусов:
                            notstarted – Не начато
                            inprogress - В процессе
                            complete - Выполнено
                            postponed - Отложено
                            failed - Провалено
                delete <номер> - удалить задачу
                update <номер> "новый текст" - обновить текст задачи
                exit - выход из программы
                """;
			Console.WriteLine(helpText);
		}
	}
}