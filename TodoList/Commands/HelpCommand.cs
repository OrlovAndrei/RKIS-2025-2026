namespace TodoList.Commands;

public class HelpCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine("""
		                  Доступные команды:
		                  help — список команд
		                  profile — выводит данные профиля
		                  profile set "name" "surname" "year" — обновляет данные профиля
		                  add "текст задачи" — добавляет задачу
		                    Флаги: --multiline -m
		                  done - отметить выполненным
		                  delete - удалить задачу
		                  view — просмотр всех задач
		                    Флаги: --index -i, --status -s, --update-date -d, --all -a
		                  exit — завершить программу
		                  read - посмотреть полный текст задачи
		                  """);
	}
}