namespace TodoList.Commands;

public class HelpCommand : ICommand
{
	public void Execute()
	{
		Console.WriteLine("СПРАВКА ПО КОМАНДАМ:");
		Console.WriteLine("help - вывести список команд");
		Console.WriteLine("profile - показать данные пользователя");
		Console.WriteLine("profile \"имя\" \"фамилия\" \"год\" - изменить данные пользователя");
		Console.WriteLine("add \"текст\" - добавить задачу");
		Console.WriteLine("add --multiline (-m) - добавить задачу в многострочном режиме");
		Console.WriteLine("view - показать только текст задач");
		Console.WriteLine("view --index (-i) - показать с индексами");
		Console.WriteLine("view --status (-s) - показать со статусами");
		Console.WriteLine("view --update-date (-d) - показать с датами");
		Console.WriteLine("view --all (-a) - показать всю информацию");
		Console.WriteLine("read <номер> - просмотреть полный текст задачи");
		Console.WriteLine("done <номер> - отметить задачу выполненной");
		Console.WriteLine("delete <номер> - удалить задачу");
		Console.WriteLine("update <номер> \"текст\" - обновить текст задачи");
		Console.WriteLine("exit - выйти из программы");
	}
}