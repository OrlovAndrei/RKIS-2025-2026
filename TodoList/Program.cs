using TodoList1;
using TodoList1.Commands;
string dataDir = Path.Combine(Environment.CurrentDirectory, "data");
string profilePath = Path.Combine(dataDir, "profile.txt");
string todoPath = Path.Combine(dataDir, "todo.csv");

FileManager.EnsureDataDirectory(dataDir);

Profile userProfile = FileManager.LoadProfile(profilePath);
if (userProfile == null)
{
	Console.WriteLine("Введите Имя:");
	string name = Console.ReadLine();
	Console.WriteLine("Введите Фамилию:");
	string surname = Console.ReadLine();
	Console.WriteLine("Введите год рождения:");
	int yearOfBirth = Convert.ToInt32(Console.ReadLine());

	userProfile = new Profile(name, surname, yearOfBirth);
	FileManager.SaveProfile(userProfile, profilePath);
	Console.WriteLine($"Создан профиль: {userProfile.GetInfo()}");
}
else
{
	Console.WriteLine($"Загружен профиль: {userProfile.GetInfo()}");
}

TodoList todoList = FileManager.LoadTodos(todoPath);
if (todoList == null)
{
	todoList = new TodoList(3);
	FileManager.SaveTodos(todoList, todoPath);
	Console.WriteLine("Создан пустой список задач.");
}
else
{
	Console.WriteLine($"Загружено задач: {todoList._count}");
}

while (true)
{
	Console.WriteLine("Введите команду: ");
	string commandInput = Console.ReadLine();
	BaseCommand command = CommandParser.Parse(commandInput, todoList, userProfile);  // todolist → todoList
	command.Execute();
}