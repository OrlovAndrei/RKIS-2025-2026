using ShevricTodo.Authentication;
using ShevricTodo.Database;

namespace ShevricTodo.Commands.Profile;

internal class Add : Profile
{
	/// <summary>
	/// Создает новый профиль на основе ввода пользователя через указанные делегаты
	/// </summary>
	/// <param name="inputString">Функция для ввода текста</param>
	/// <param name="inputDateTime">Функция для ввода даты</param>
	/// <param name="inputBool">Функция для выбора добавлять или нет</param>
	/// <param name="inputPassword">Функция для ввода пароля</param>
	/// <param name="firstName"></param>
	/// <param name="lastName"></param>
	/// <param name="userName"></param>
	/// <param name="birthday"></param>
	/// <returns></returns>
	public static async Task<(int resultSave, Database.Profile profile)> Done(
		Func<string, string?> inputString,
		Func<string, DateTime?> inputDateTime,
		Func<string, bool> inputBool,
		Func<string> inputPassword,
		string? firstName = null,
		string? lastName = null,
		string? userName = null,
		DateTime? birthday = null)
	{
		DateTime nowDateTime = DateTime.Now;
		Database.Profile newProfile = new()
		{
			// если не было передано в качестве аргумента тогда предложит пользователю ввести его
			FirstName = firstName ?? inputString("Введите ваше имя: "),
			// аналогично
			LastName = lastName ?? inputString("Введите вашу фамилию: "),
			UserName = userName ?? (inputBool("Желаете ввести псевдоним? ")
				? inputString("Введите ваш псевдоним: ")
				: null),
			Birthday = inputDateTime("Введите ваш день рождения: "),
			DateOfCreate = nowDateTime,
			HashPassword = await Encryption.CreatePasswordHash(inputPassword(), nowDateTime)
		};
		return (await AddNew(newProfile), newProfile);
	}
}
