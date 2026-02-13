namespace ShevricTodo.Commands.ProfileObj;

internal partial class Add
{
	public static async Task<(int resultSave, Database.Profile newProfile)> Done(
		Database.Profile newProfile) => await Done(
			inputString: Input.Text.ShortText,
			inputDateTime: Input.When.Date,
			inputBool: Input.Button.YesOrNo,
			inputPassword: Input.Password.CheckingThePassword,
			newProfile: newProfile);
}
