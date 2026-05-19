using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace TodoList.Server
{
	class Program
	{
		private const string Url = "http://localhost:5000/";
		private static TodoRepository _repo;

		static async Task Main(string[] args)
		{
			using var listener = new HttpListener();
			listener.Prefixes.Add(Url);
			listener.Start();

			var db = new Database("Data Source=Data/todolist.db");
			_repo = new TodoRepository(db);
			await _repo.InitializeAsync();

			Console.WriteLine("=== СЕРВЕР ЗАПУЩЕН ===");
			Console.WriteLine($"Ожидание на {Url}");

			while (true)
			{
				var context = await listener.GetContextAsync();
				_ = Task.Run(() => HandleRequest(context));
			}
		}

		static async Task HandleRequest(HttpListenerContext context)
		{
			var request = context.Request;
			var response = context.Response;
			string path = request.Url.AbsolutePath.Trim('/');

			try
			{
				string table = path.StartsWith("profiles") ? "Users" : "Todos";
				string dataId = path.StartsWith("todos/") ? path.Replace("todos/", "") : "global_profile";

				if (request.HttpMethod == "POST")
				{
					using var ms = new MemoryStream();
					request.InputStream.CopyTo(ms);
					byte[] data = ms.ToArray();

					await _repo.SaveDataAsync(table, dataId, data);
					response.StatusCode = (int)HttpStatusCode.OK;
				}
				else if (request.HttpMethod == "GET")
				{
					byte[] data = await _repo.GetDataAsync(table, dataId);

					if (data != null)
					{
						response.ContentLength64 = data.Length;
						response.OutputStream.Write(data, 0, data.Length);
					}
					else
					{
						response.StatusCode = (int)HttpStatusCode.NotFound;
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка базы данных: {ex.Message}");
				response.StatusCode = (int)HttpStatusCode.InternalServerError;
			}
			finally
			{
				response.Close();
			}
		}
	}
}