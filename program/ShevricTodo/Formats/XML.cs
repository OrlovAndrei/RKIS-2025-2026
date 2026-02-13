using System.Runtime.Serialization;
using System.Xml;

namespace ShevricTodo.Formats;

public class Xml<T> : FileSerializationFormat, ISerializationFormat<T>
{
	public const string FileExtension = ".xml";
	public static T? Deserialization(string path)
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
	public T? Deserialization()
	{
		IsPathNull();
		return Deserialization(Path!);
	}
	public static void Serialization(T value, string path)
	{
		DataContractSerializer xmlSerializer = new(typeof(T));
		using (XmlWriter stream = XmlWriter.Create(path))
		{
			xmlSerializer.WriteObject(stream, value);
		}
	}
	public void Serialization(T value)
	{
		IsPathNull();
		Serialization(value, Path!);
	}
}
