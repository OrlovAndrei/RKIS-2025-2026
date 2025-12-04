using YamlDotNet.Serialization;

namespace TodoList;

public class DataType
{
    public string Name { get; set; } = "";
    public string[] SpellingOptions { get; set; } = new string[0];
}

public class SearchDataTypeOnJson
{
    private static IDeserializer deserializer = new DeserializerBuilder()
        .Build();
    private static string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataTypeAndCommands", "DataType.yaml");
    public static List<DataType> DataTypes = deserializer.Deserialize<List<DataType>>(File.OpenText(fullPath));
    public static string ConvertingInputValues(string inputValue)
	{
		if (inputValue.Length != 0 && DataTypes != null &&
		DataTypes.Count() > 0)
		{
			foreach (var dataType in DataTypes)
			{
				if (dataType.Name.Length != 0 && dataType.SpellingOptions.Length > 0 &&
				dataType.SpellingOptions.Contains(inputValue))
				{
					return dataType.Name;
				}
			}
		}
		return "";
	}

}
