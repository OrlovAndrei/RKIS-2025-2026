namespace TodoList.commands;

public class HelpCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine(
			"""
			Доступные команды:
			help — список команд
			profile — выводит данные профиля
			set profile (name, surname, bday) — ввод данных профиля
			add "текст задачи" — добавляет задачу
			-m, --multi — добавление задачи в несколько строк
			view — просмотр всех задач
			-a, --all — добавить все поля таблицы
			-i, --index — добавить поле индекса
			-s, --status — добавить поле статуса
			-d, --update-date — добавить поле даты
			status <индекс> <enum> — изменить статус задачи
			delete <индекс> — удалить задачу
			update <индекс> "новый текст" — изменить текст задачи
			read <индекс> — просмотр задачи
			exit — завершить программу
			""");
	}
}