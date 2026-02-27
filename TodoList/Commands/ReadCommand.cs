namespace TodoApp.Commands
{
	public class ReadCommand : BaseCommand, ICommand
	{
		public TodoList TodoList { get; set; }
		public int Index { get; set; }
		public ReadCommand()
		{
			TodoList = AppInfo.Todos;
		}
		public ReadCommand(int index) : this()
		{
			if (index < 0)
			{
				throw new ArgumentException("Индекс задачи должен быть неотрицательным.");
			}
			Index = index;
		}
		public override void Execute()
		{
			var item = TodoList.GetItem(Index);
			if (item == null)
			{
				Console.WriteLine($"Ошибка: задача с номером {Index + 1} не найдена.");
				return;
			}
			Console.WriteLine(item.GetFullInfo());
		}
	}
}
