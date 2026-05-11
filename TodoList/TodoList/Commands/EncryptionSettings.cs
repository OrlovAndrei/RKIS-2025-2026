using System.Security.Cryptography;
using System.Text;
public static class EncryptionSettings
{
	public static readonly byte[] Key = Encoding.UTF8.GetBytes("1234567890123456");
	public static readonly byte[] IV = Encoding.UTF8.GetBytes("6543210987654321");
	static EncryptionSettings()
	{
		if (Key.Length != 16 && Key.Length != 24 && Key.Length != 32)
		{
			throw new CryptographicException("Encryption key must be 16, 24, or 32 bytes long.");
		}
		if (IV.Length != 16)
		{
			throw new CryptographicException("Encryption IV must be 16 bytes long.");
		}
	}
}
