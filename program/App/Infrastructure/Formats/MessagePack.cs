using MessagePack;

namespace ShevricTodo.Formats;

public class MessagePack<T> : FileSerializationFormat, ISerializationFormat<T>
{
	public const string FileExtension = ".bin";
	public static async Task<T?> DeserializationAsync(string path)
	{
		IsFileExist(path);
		using (Stream stream = File.Open(path, FileMode.Open))
		{
			return await MessagePackSerializer.DeserializeAsync<T>(stream);
		}
	}
	public async Task<T?> DeserializationAsync()
	{
		IsPathNull();
		return await DeserializationAsync(Path!);
	}
	public static async Task SerializationAsync(T value, string path)
	{
		using (Stream stream = File.Open(path, FileMode.OpenOrCreate))
		{
			await MessagePackSerializer.SerializeAsync<T>(stream, value);
		}
	}
	public async Task SerializationAsync(T value)
	{
		IsPathNull();
		await SerializationAsync(value, Path!);
	}
}
