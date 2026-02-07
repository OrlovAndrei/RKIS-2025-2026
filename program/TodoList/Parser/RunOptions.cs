namespace ShevricTodo.Parser;

internal class RunOptions
{
	public async static void Run(object obj)
	{
		switch (obj)
		{
			case Verb.Task t:
				if (t.Add)
				{
					await Commands.TaskVerb.Add.Done(
						inputStringShort: Input.Text.ShortText,
						inputStringLong: Input.Text.LongText,
						inputBool: Input.Button.YesOrNo,
						inputDateTime: Input.When.DateAndTime,
						inputOneOf: Input.OneOf.GetOneFromList,
						searchTemplate: new Database.TaskTodo());
				}
				break;
			case Verb.Profile p:
				//
				break;
			case Verb.Redo r:
				//
				break;
			case Verb.Undo u:
				//
				break;
			case Verb.Exit e:
				//
				break;
			case Verb.Run e:
				await Program.Run();
				break;
		}
	}
}
