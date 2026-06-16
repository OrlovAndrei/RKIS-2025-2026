namespace TodoList.Commands;
public class HelpCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine(
			@"Доступные команды:
            help - вывести список команд
            profile - показать данные пользователя
            add текст задачи - добавить новую задачу (флаги: --multiline -m)
            view - показать все задачи (флаги: --all -a, --index -i, --status -s, --update-date -d)
            read idx - просмотр задачи
	            done idx - отмечает задачу выполненной
            delete idx - удаляет задачу
	            update idx - обновляет задачу
            exit - выйти из программы"
		);
	}
}