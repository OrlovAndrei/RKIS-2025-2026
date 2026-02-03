using System.Text.Json;
using System.Text.Json.Serialization;

namespace ShevricTodo.Formats;

public class Json<T> : FileSerializationFormat, ISerializationFormat<T>
{
	public const string FileExtension = ".json";
	public JsonSerializerOptions serializerOptions = JsonSerializerOptions.Default;
	public Json(string path)
	{
		Path = path;
	}
	public static T? Deserialization(string path)
	{
		IsFileExist(path);
		using (Stream stream = File.Open(path, FileMode.Open))
		{
			return JsonSerializer.Deserialize<T>(stream);
		}
	}
	public T? Deserialization()
	{
		IsPathNull();
		return Deserialization(Path!);
	}
	public static void Serialization(T value, string path, JsonSerializerOptions? serializerOptions = null)
	{
		if (serializerOptions is null)
		{
			serializerOptions = JsonSerializerOptions.Default;
		}
		using (Stream stream = File.Create(path))
		{
			JsonSerializer.Serialize(stream, value, serializerOptions);
		}
	}
	public void Serialization(T value)
	{
		IsPathNull();
		Serialization(value, Path!, this.serializerOptions);
	}
}
