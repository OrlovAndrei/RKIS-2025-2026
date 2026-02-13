using ShevricTodo.Database;

namespace ShevricTodo.Commands.TaskObj;

internal partial class Add
{
	public static async Task<(int resultSave, TaskTodo taskTodo)> Done(
		TaskTodo searchTemplate) => await Done(
			inputStringShort: Input.Text.ShortText,
			inputStringLong: Input.Text.LongText,
			inputDateTime: Input.When.DateAndTime,
			inputBool: Input.Button.YesOrNo,
			inputOneOf: Input.OneOf.GetOneFromList,
			searchTemplate: searchTemplate);
}
