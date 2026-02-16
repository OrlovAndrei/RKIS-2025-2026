using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskObj;

internal partial class Add : TaskObj
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
	private static async Task<(int resultSave, TaskTodo taskTodo)> Done(
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
		await searchTemplate.EnteringName(
			inputStringShort: inputStringShort, 
			message: "Введите название задачи: ");
		await searchTemplate.EnteringDescription(
			inputStringLong: inputStringLong,
			message: "Введите описание задачи: ");
		await searchTemplate.EnteringDeadline(
			inputBool: inputBool,
			inputDateTime: inputDateTime,
			messageQuestion: "Желаете ввести крайний срок на выполнение задачи? ",
			message: "Введите крайний срок на выполнение задачи");
		await searchTemplate.EnteringState(inputOneOf);
		await searchTemplate.EnteringType(inputOneOf);
		await searchTemplate.EnteringDateOfCreate();
		await searchTemplate.EnteringUserId();
		return (await AddNewTask(searchTemplate), searchTemplate);
	}
}
