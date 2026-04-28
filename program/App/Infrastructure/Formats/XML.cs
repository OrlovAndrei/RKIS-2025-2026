using System.Runtime.Serialization;
using System.Xml;

namespace ShevricTodo.Formats;

public class Xml<T> : FileSerializationFormat, ISerializationFormat<T>
{
	public const string FileExtension = ".xml";
	public static async Task<T?> DeserializationAsync(string path)
	{
		IsFileExist(path);
		DataContractSerializer xmlSerializer = new(typeof(T));
		using (XmlReader stream = XmlReader.Create(path))
		{
			object? result = xmlSerializer.ReadObject(stream);
			if (result is not null)
			{
				return (T)result;
			}
			else
			{
				throw new XmlException();
			}
		}
	}
	public async Task<T?> DeserializationAsync()
	{
		IsPathNull();
		return await DeserializationAsync(Path!);
	}
	public static async Task SerializationAsync(T value, string path)
	{
		DataContractSerializer xmlSerializer = new(typeof(T));
		using (XmlWriter stream = XmlWriter.Create(path))
		{
			xmlSerializer.WriteObject(stream, value);
		}
	}
	public async Task SerializationAsync(T value)
	{
		IsPathNull();
		await SerializationAsync(value, Path!);
	}
}
