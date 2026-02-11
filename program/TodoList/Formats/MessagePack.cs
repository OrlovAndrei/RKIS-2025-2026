using MessagePack;

namespace ShevricTodo.Formats;

public class MessagePack<T> : FileSerializationFormat, ISerializationFormat<T>
{
	public const string FileExtension = ".bin";
	public static T? Deserialization(string path)
	{
		IsFileExist(path);
		using (Stream stream = File.Open(path, FileMode.Open))
		{
			return MessagePackSerializer.Deserialize<T>(stream);
		}
	}
	public T? Deserialization()
	{
		IsPathNull();
		return Deserialization(Path!);
	}
	public static void Serialization(T value, string path)
	{
		using (Stream stream = File.Open(path, FileMode.OpenOrCreate))
		{
			MessagePackSerializer.Serialize<T>(stream, value);
		}
	}
	public void Serialization(T value)
	{
		IsPathNull();
		Serialization(value, Path!);
	}
}
