

namespace TodoList;
internal class TestCommand : ICommand
{
	private static ITestCase ParseTestCase(string name)
	{
		if (name == "prof_create") return new TestProfCreate();
		else if (name == "prof_ent") return new TestProfEnter();
		return new UncorrectTest();
	}

	public void Execute()
	{
		Console.WriteLine("*************************** Testing ***************************");

		Console.WriteLine(
			"""
			Создание профиля - prof_create
			Вход в созданный профиль - prof_ent
			""");
			

		Console.WriteLine("Enter test case name...");
		var testCaseName = Console.ReadLine();

		var testCase = ParseTestCase(testCaseName);
		testCase.Run();

		Console.WriteLine("***************************************************************");
	}

	public void Unexecute() { }
}
