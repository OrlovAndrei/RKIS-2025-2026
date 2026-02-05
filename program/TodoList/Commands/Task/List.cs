using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.Task;

internal class List : Task
{
	/// <summary>
	/// Асинхронная функция выводящая в консоль таблицу всех задач активного пользователя
	/// </summary>
	/// <param name="printTable">Метод для выводы табличных данных</param>
	/// <returns></returns>
	public static async System.Threading.Tasks.Task PrintAllTasks(
		Action<string[], IEnumerable<string[]>, string?> printTable)
	{
		IEnumerable<TaskTodo> allTasks = await GetAllTasks();
		IEnumerable<TypeOfTask> allTypes = await GetAllTypeOfTask();
		IEnumerable<StateOfTask> allStates = await GetAllStateOfTask();
		Database.Profile activeUser = await ActiveProfile.GetActiveProfile();
		string title = $"{activeUser.UserId}: {activeUser.FirstName} {activeUser.LastName}";
		string[] columns = [
			"TaskId",
			"TapeOfTask",
			"StateOfTask",
			"Name",
			"Description",
			"DataOfCreate",
			"DateOfStart",
			"DataOfEnd",
			"Deadline"];
		IEnumerable<string[]> rows =
			from task in allTasks
			join type in allTypes on task.TypeId equals type.TypeId
			join state in allStates on task.StateId equals state.StateId
			orderby task.TaskId
			select new string[]
			{
				task.TaskId.ToString(),
				type.Name ?? "N/A",
				state.Name ?? "N/A",
				task.Name ?? "N/A",
				task.Description ?? "N/A",
				task.DateOfCreate.ToString(),
				task.DateOfStart.ToString() ?? "N/A",
				task.DateOfEnd.ToString() ?? "N/A",
				task.Deadline.ToString() ?? "N/A"
			}
			.ToArray();
		printTable(columns, rows, title);
	}
}
