namespace TodoList;

public class HelpCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine("""
		                  Доступные команды:
		                  help — список команд
		                  profile — выводит данные профиля
		                  add "текст задачи" — добавляет задачу
		                    Флаги: --multiline -m
		                  done <номер> — отметить выполненным
		                  delete <номер> — удалить задачу
		                  view — просмотр задач
		                    Флаги: --index -i, --status -s, --update-date -d, --all -a
		                  exit — завершить программу
		                  read <номер> — полный текст задачи
		                  """);
	}
}