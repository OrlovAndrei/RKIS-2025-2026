using System.Text;

namespace ShevricTodo.Commands.TaskObj;

internal partial class Show : TaskObj
{
	/// <summary>
	/// Displays detailed information about a specified task, including its profile, type, state, and relevant dates, using
	/// the provided panel printing action.
	/// </summary>
	/// <remarks>This method retrieves the profile, type, and state of the task asynchronously before printing the
	/// details. The panel includes information such as the task's ID, name, description, creation date, start and end
	/// dates, and deadline if available.</remarks>
	/// <param name="printPanel">An action that receives a header string and a collection of text lines to display the task details in a panel
	/// format.</param>
	/// <param name="task">The task whose details are to be displayed. Must not be null.</param>
	/// <returns>A task that represents the asynchronous operation of displaying the task details.</returns>
	private static async Task ShowTask(
		Func<string?, IEnumerable<string>, Task> printPanel,
		Database.TaskTodo task)
	{
		Database.Profile profile = await GetProfileOfTask(task);
		Database.TypeOfTask type = await GetTypeOfTask(task);
		Database.StateOfTask state = await GetStateOfTask(task);
		StringBuilder header = new($" ID: {task.TaskId} ");
		List<string> textLinesPanel =
		[
			$"Profile: {profile.FirstName} {profile.LastName}",
			$"Type: {type.Name}",
			$"State: {state.Name}",
			$"DateOfCreate: {task.DateOfCreate}.",
		];
		if (task.Name is not null)
		{
			textLinesPanel.Add($"NameTask: {task.Name}.");
			header.Append($" {task.Name} ");
		}
		if (task.Description is not null)
		{ textLinesPanel.Add($"Description: {task.Description}."); }
		if (task.DateOfStart is not null)
		{ textLinesPanel.Add($"DateOfStart: {task.DateOfStart}."); }
		if (task.DateOfEnd is not null)
		{ textLinesPanel.Add($"DateOfEnd: {task.DateOfEnd}."); }
		if (task.Deadline is not null)
		{ textLinesPanel.Add($"Deadline: {task.Deadline}."); }
		await printPanel(header.ToString(), textLinesPanel);
	}
}
