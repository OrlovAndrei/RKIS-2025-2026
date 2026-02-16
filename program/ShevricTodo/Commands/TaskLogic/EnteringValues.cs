using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskObj;

internal static class EnteringValues
{
	internal static async Task<TaskTodo> EnteringState(
		this TaskTodo template, Func<Dictionary<int, string>, string?, int, KeyValuePair<int, string>> inputOneOf)
	{
		if (!template.StateId.HasValue)
		{
			template.StateId = inputOneOf(await TaskObj.GetAllStates(), null, 3).Key;
		}
		return template;
	}
	internal static async Task<TaskTodo> EnteringType(
		this TaskTodo template, Func<Dictionary<int, string>, string?, int, KeyValuePair<int, string>> inputOneOf)
	{
		if (!template.TypeId.HasValue)
		{
			template.TypeId = inputOneOf(await TaskObj.GetAllTypes(), null, 3).Key;
		}
		return template;
	}
	internal static async Task<TaskTodo> EnteringDateOfCreate(
		this TaskTodo template)
	{
		template.DateOfCreate = DateTime.Now;
		return template;
	}
	internal static async Task<TaskTodo> EnteringUserId(
		this TaskTodo template)
	{
		template.UserId = (await ActiveProfile.Read() ?? throw new Exception()).UserId;
		return template;
	}
	internal static async Task<TaskTodo> EnteringName(
		this TaskTodo template, 
		string message,
		Func<string, string?> inputStringShort)
	{
		template.Name ??= inputStringShort(message);
		return template;
	}
	internal static async Task<TaskTodo> EnteringDescription(
		this TaskTodo template, 
		string message,
		Func<string, string?> inputStringLong)
	{
		template.Description ??= inputStringLong(message);
		return template;
	}
	internal static async Task<TaskTodo> EnteringDeadline(
		this TaskTodo template, 
		Func<string, bool> inputBool, 
		string messageQuestion,
		string message,
		Func<string, DateTime?> inputDateTime)
	{
		if (template.Deadline is null && inputBool(messageQuestion))
		{
			template.Deadline = inputDateTime(message);
		}
		return template;
	}
}