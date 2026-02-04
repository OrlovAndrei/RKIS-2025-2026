
namespace TodoList
{
    internal class HelpCommand : ICommand
	{
		public void Execute()
		{
			Console.WriteLine($"""
				help - список команд
				profile - данные пользователя, -o --out чтобы сменить профиль
				add - добавить задачу
				add -m - добавить задачу в многострочном режиме
				view -i, -s, -d, -a - показать задачи
				read - просмотреть полный текст задачи
				delete - удалить задачу
				update - обновить задачу
				undo - отмена действия
				redo - повтор действия
				status - <idx> изменить статус задачи {TodoStatus.Completed}, {TodoStatus.Failed}, {TodoStatus.NotStarted}, {TodoStatus.InProgress}
				exit - выход
				"""); 
		}

		public void Unexecute() { }
	}
}
