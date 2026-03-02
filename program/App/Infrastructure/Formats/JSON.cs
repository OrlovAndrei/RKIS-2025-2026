using System.Text.Json;

namespace ShevricTodo.Formats;

public class Json<T> : FileSerializationFormat, ISerializationFormat<T>
{
	public const string FileExtension = ".json";
	public JsonSerializerOptions SerializerOptions = JsonSerializerOptions.Default;
	public Json(string path)
	{
		Path = path;
	}
	public static async Task<T?> DeserializationAsync(string path)
	{
		IsFileExist(path);
		using (Stream stream = File.Open(path, FileMode.Open))
		{
			return await JsonSerializer.DeserializeAsync<T>(stream);
		}
	}
	public async Task<T?> DeserializationAsync()
	{
		IsPathNull();
		return await DeserializationAsync(Path!);
	}
	public static async Task SerializationAsync(T value, string path, JsonSerializerOptions? serializerOptions = null)
	{
		if (serializerOptions is null)
		{
			serializerOptions = JsonSerializerOptions.Default;
		}
		using (Stream stream = File.Create(path))
		{
			await JsonSerializer.SerializeAsync(stream, value, serializerOptions);
		}
	}
	public async Task SerializationAsync(T value)
	{
		IsPathNull();
		await SerializationAsync(value, Path!, SerializerOptions);
	}
}
