namespace ShevricTodo.Commands.ProfileObj;

internal partial class Add : ProfileObj
{
	/// <summary>
	/// Processes user input to create or update a user profile asynchronously, returning the result of the save operation
	/// and the updated profile.
	/// </summary>
	/// <remarks>The method collects profile information interactively from the user and ensures that the password
	/// is securely hashed before saving. The profile's creation date is set to the current time. If an existing profile is
	/// provided, only missing fields are filled with new input.</remarks>
	/// <param name="inputString">A function that prompts the user for a string input, used to obtain the first name, last name, and optionally the
	/// username.</param>
	/// <param name="inputDateTime">A function that prompts the user for a date input, used to obtain the user's birthday.</param>
	/// <param name="inputBool">A function that prompts the user for a boolean input, used to determine whether the user wishes to provide a
	/// username.</param>
	/// <param name="inputPassword">A function that prompts the user for a password input, used to obtain and hash the user's password.</param>
	/// <param name="newProfile">An optional profile instance to update. If null, a new profile is created and populated with user input.</param>
	/// <returns>A tuple containing the result of the save operation as an integer and the updated user profile.</returns>
	private static async Task<(int resultSave, Database.Profile newProfile)> Done(
		Func<string, string?> inputString,
		Func<string, DateTime?> inputDateTime,
		Func<string, bool> inputBool,
		Func<string> inputPassword,
		Database.Profile newProfile)
	{
		await newProfile.EnteringFirstName(inputString: inputString);
		await newProfile.EnteringLastName(inputString: inputString);
		await newProfile.EnteringUserName(inputBool: inputBool, inputString: inputString);
		await newProfile.EnteringBirthday(inputDateTime: inputDateTime);
		await newProfile.EnteringDateOfCreate();
		await newProfile.EnteringNewPassword(inputPassword: inputPassword);
		return (await AddNewProfile(newProfile), newProfile);
	}
}
