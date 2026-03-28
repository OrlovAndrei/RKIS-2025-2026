using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
namespace TodoList.Server
{
    public class Program
    {
		private static readonly string ServerUrl = "http://localhost:5000/";
		static async Task Main(string[] args)
		{
			Console.WriteLine("Запуск сервера...");
			using (HttpListener listener = new HttpListener())
			{
				listener.Prefixes.Add(ServerUrl);
				listener.Start();
				Console.WriteLine($"Сервер запущен на {ServerUrl}");
				while (true)
				{
					try
					{
						HttpListenerContext context = await listener.GetContextAsync();
						_ = Task.Run(() => ProcessRequestAsync(context));
					}
					catch (Exception ex)
					{
						Console.WriteLine($"Ошибка: {ex.Message}");
					}
				}
			}
		}
		private static async Task ProcessRequestAsync(HttpListenerContext context)
		{
			HttpListenerRequest request = context.Request;
			HttpListenerResponse response = context.Response;
			Console.WriteLine($"{request.HttpMethod} {request.Url.AbsolutePath}");
			try
			{
				if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/profiles")
				{
					await HandlePostProfiles(request, response);
				}
				else if (request.HttpMethod == "GET" && request.Url.AbsolutePath == "/profiles")
				{
					await HandleGetProfiles(response);
				}
				else if (request.HttpMethod == "POST" && request.Url.AbsolutePath.StartsWith("/todos/"))
				{
					await HandlePostTodos(request, response);
				}
				else if (request.HttpMethod == "GET" && request.Url.AbsolutePath.StartsWith("/todos/"))
				{
					await HandleGetTodos(request, response);
				}
				else
				{
					response.StatusCode = 404;
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes("Not Found");
					await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
				}
			}
			catch (Exception ex)
			{
				response.StatusCode = 500;
				byte[] buffer = System.Text.Encoding.UTF8.GetBytes($"Server Error: {ex.Message}");
				await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
			}
			finally
			{
				response.Close();
			}
		}
		private static async Task HandlePostProfiles(HttpListenerRequest request, HttpListenerResponse response)
		{
			byte[] buffer = new byte[request.ContentLength64];
			await request.InputStream.ReadAsync(buffer, 0, buffer.Length);
			File.WriteAllBytes("server_profiles.dat", buffer);
			response.StatusCode = 200;
			byte[] okBuffer = System.Text.Encoding.UTF8.GetBytes("OK");
			await response.OutputStream.WriteAsync(okBuffer, 0, okBuffer.Length);
		}
		private static async Task HandleGetProfiles(HttpListenerResponse response)
		{
			if (File.Exists("server_profiles.dat"))
			{
				byte[] data = File.ReadAllBytes("server_profiles.dat");
				response.ContentType = "application/octet-stream";
				response.ContentLength64 = data.Length;
				await response.OutputStream.WriteAsync(data, 0, data.Length);
			}
			else
			{
				response.ContentLength64 = 0;
			}
		}
		private static async Task HandlePostTodos(HttpListenerRequest request, HttpListenerResponse response)
		{
			string path = request.Url.AbsolutePath;
			string userId = path.Replace("/todos/", "");
			byte[] buffer = new byte[request.ContentLength64];
			await request.InputStream.ReadAsync(buffer, 0, buffer.Length);
			string filename = $"server_todos_{userId}.dat";
			File.WriteAllBytes(filename, buffer);
			response.StatusCode = 200;
			byte[] okBuffer = System.Text.Encoding.UTF8.GetBytes("OK");
			await response.OutputStream.WriteAsync(okBuffer, 0, okBuffer.Length);
		}
		private static async Task HandleGetTodos(HttpListenerRequest request, HttpListenerResponse response)
		{
			string path = request.Url.AbsolutePath;
			string userId = path.Replace("/todos/", "");
			string filename = $"server_todos_{userId}.dat";
			if (File.Exists(filename))
			{
				byte[] data = File.ReadAllBytes(filename);
				response.ContentType = "application/octet-stream";
				response.ContentLength64 = data.Length;
				await response.OutputStream.WriteAsync(data, 0, data.Length);
			}
			else
			{
				response.ContentLength64 = 0;
			}
		}
	}
}
