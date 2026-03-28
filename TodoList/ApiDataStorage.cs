using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TodoApp.Commands;
namespace TodoList;
public class ApiDataStorage : IDataStorage
{
	private readonly HttpClient _httpClient;
	private readonly string _serverUrl = "http://localhost:5000/";

	public ApiDataStorage()
	{
		_httpClient = new HttpClient();
		_httpClient.BaseAddress = new Uri(_serverUrl);
	}
}
