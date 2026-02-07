using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskVerb;

internal class Add : TaskObj
{
	/// <summary>
	/// Creates a new task with the specified parameters, marks it as completed, and saves it to the database
	/// asynchronously.
	/// </summary>
	/// <remarks>If optional parameters are not provided, the method will prompt the user for input using the
	/// supplied functions. The task is associated with the currently active user profile.</remarks>
	/// <param name="inputStringShort">A function that prompts the user for a short string input, typically used to obtain the task name if not provided.</param>
	/// <param name="inputStringLong">A function that prompts the user for a long string input, typically used to obtain the task description if not
	/// provided.</param>
	/// <param name="inputDateTime">A function that prompts the user for a date and time input, used to specify the task's deadline if required.</param>
	/// <param name="inputBool">A function that prompts the user for a boolean input, indicating whether the user wishes to set a deadline for the
	/// task.</param>
	/// <param name="inputOneOf">A function that allows the user to select one value from a dictionary of options, used for choosing the task's
	/// state and type.</param>
	public static async Task<(int resultSave, TaskTodo taskTodo)> Done(
		Func<string, string?> inputStringShort,
		Func<string, string?> inputStringLong,
		Func<string, DateTime?> inputDateTime,
		Func<string, bool> inputBool,
		Func<Dictionary<int, string>,
			string?,
			int,
			KeyValuePair<int, string>> inputOneOf,
		TaskTodo searchTemplate)
	{
		DateTime nowDateTime = DateTime.Now;
		TaskTodo newTask = new()
		{
			Name = searchTemplate.Name ?? inputStringShort("Введите название задачи: "),
			StateId = inputOneOf(await GetAllStates(), null, 3).Key,
			TypeId = inputOneOf(await GetAllTypes(), null, 3).Key,
			Description = searchTemplate.Description ?? inputStringLong("Введите описание задачи: "),
			Deadline = searchTemplate.Deadline ?? (inputBool("Желаете ввести крайний срок на выполнение задачи? ")
				? inputDateTime("Введите крайний срок на выполнение задачи")
				: null),
			DateOfCreate = nowDateTime,
			UserId = ActiveProfile.Read().Id,
		};
		return (await AddNew(newTask), newTask);
	}
	public static async Task<(int resultSave, TaskTodo taskTodo)> Done(
		TaskTodo searchTemplate)
	{
		return await Done(
			inputStringShort: Input.Text.ShortText,
			inputStringLong: Input.Text.LongText,
			inputDateTime: Input.When.DateAndTime,
			inputBool: Input.Button.YesOrNo,
			inputOneOf: Input.OneOf.GetOneFromList,
			searchTemplate: searchTemplate);
	}
}
