using System.Security.Cryptography;
using System.Text;

namespace ShevricTodo.Authentication;

internal static class Encryption
{
	public static string CreateMD5(string input)
	{
#pragma warning disable IDE1006 // Стили именования
		MD5 MD5Hash = MD5.Create(); //создаем объект для работы с MD5
#pragma warning restore IDE1006 // Стили именования
		byte[] inputBytes = Encoding.ASCII.GetBytes(input); //преобразуем строку в массив байтов
		byte[] hash = MD5Hash.ComputeHash(inputBytes); //получаем хэш в виде массива байтов
		return Convert.ToHexString(hash); //преобразуем хэш из массива в строку, состоящую из шестнадцатеричных символов в верхнем регистре
	}
	public static string CreateSHA256(params string[] input)
	{
		using SHA256 hash = SHA256.Create();
		return Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(string.Join(string.Empty, )));
	}
	private static string NowDateForHash() => $"{DateTime.Now:ssffffff}";
	private static int RandomSalt()
	{
		int.TryParse(NowDateForHash(), out int seed);
		seed += new Random(Seed: seed).Next();
		return seed;
	}
	private static Int64 CreateUID()
	{
		Random random = new Random(RandomSalt());
		return random.NextInt64();
	}

}
