using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace TodoList.Server
{
	public class Program
	{
		private static readonly string SERVER_URL = "http://localhost:5000/";
		private static readonly string STORAGE_DIR = "server_storage";
		public static void Main(string[] args)
		{
			if (!Directory.Exists(STORAGE_DIR))
				Directory.CreateDirectory(STORAGE_DIR);
			Console.WriteLine("=== HTTP СЕРВЕР ДЛЯ СИНХРОНИЗАЦИИ ===");
			Console.WriteLine($"Сервер запускается на {SERVER_URL}");
			Console.WriteLine($"Данные будут сохраняться в папку: {STORAGE_DIR}");
			Console.WriteLine("========================================");
			RunServerAsync().GetAwaiter().GetResult();
		}
		private static async Task RunServerAsync()
		{
			using (HttpListener listener = new HttpListener())
			{
				listener.Prefixes.Add(SERVER_URL);
				listener.Start();
				Console.WriteLine($"✅ СЕРВЕР ЗАПУЩЕН на {SERVER_URL}");
				Console.WriteLine("Ожидание запросов...\n");
				while (true)
				{
					try
					{
						HttpListenerContext context = await listener.GetContextAsync();
						_ = Task.Run(() => ProcessRequestAsync(context));
					}
					catch (Exception ex)
					{
						Console.WriteLine($"❌ Ошибка при получении запроса: {ex.Message}");
					}
				}
			}
		}
		private static async Task ProcessRequestAsync(HttpListenerContext context)
		{
			HttpListenerRequest request = context.Request;
			HttpListenerResponse response = context.Response;
			Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {request.HttpMethod} {request.Url.AbsolutePath}");
			try
			{
				if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/profiles")
				{
					await HandleSaveProfiles(request, response);
				}
				else if (request.HttpMethod == "GET" && request.Url.AbsolutePath == "/profiles")
				{
					await HandleLoadProfiles(response);
				}
				else if (request.HttpMethod == "POST" && request.Url.AbsolutePath.StartsWith("/todos/"))
				{
					await HandleSaveTodos(request, response);
				}
				else if (request.HttpMethod == "GET" && request.Url.AbsolutePath.StartsWith("/todos/"))
				{
					await HandleLoadTodos(request, response);
				}
				else
				{
					response.StatusCode = 404;
					string errorMsg = "Команда не найдена - доступные endpoint: POST /profiles, GET /profiles, POST /todos/{userId}, GET /todos/{userId}";
					byte[] errorBuffer = Encoding.UTF8.GetBytes(errorMsg);
					response.ContentLength64 = errorBuffer.Length;
					await response.OutputStream.WriteAsync(errorBuffer, 0, errorBuffer.Length);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"❌ Ошибка обработки: {ex.Message}");
				response.StatusCode = 500;
				byte[] errorBuffer = Encoding.UTF8.GetBytes($"Server Error: {ex.Message}");
				response.ContentLength64 = errorBuffer.Length;
				await response.OutputStream.WriteAsync(errorBuffer, 0, errorBuffer.Length);
			}
			finally
			{
				response.Close();
			}
		}
		private static async Task HandleSaveProfiles(HttpListenerRequest request, HttpListenerResponse response)
		{
			byte[] encryptedData = new byte[request.ContentLength64];
			await request.InputStream.ReadAsync(encryptedData, 0, encryptedData.Length);
			string filePath = Path.Combine(STORAGE_DIR, "profiles.dat");
			File.WriteAllBytes(filePath, encryptedData);
			Console.WriteLine($"   📁 Сохранены профили: {encryptedData.Length} байт");
			response.StatusCode = 200;
			byte[] okBuffer = Encoding.UTF8.GetBytes("OK");
			response.ContentLength64 = okBuffer.Length;
			await response.OutputStream.WriteAsync(okBuffer, 0, okBuffer.Length);
		}
		private static async Task HandleLoadProfiles(HttpListenerResponse response)
		{
			string filePath = Path.Combine(STORAGE_DIR, "profiles.dat");
			if (File.Exists(filePath))
			{
				byte[] encryptedData = File.ReadAllBytes(filePath);
				response.ContentType = "application/octet-stream";
				response.ContentLength64 = encryptedData.Length;
				await response.OutputStream.WriteAsync(encryptedData, 0, encryptedData.Length);
				Console.WriteLine($"   📂 Отданы профили: {encryptedData.Length} байт");
			}
			else
			{
				response.ContentLength64 = 0;
				Console.WriteLine("   ⚠️ Файл профилей не найден");
			}
			response.StatusCode = 200;
		}
		private static async Task HandleSaveTodos(HttpListenerRequest request, HttpListenerResponse response)
		{
			string path = request.Url.AbsolutePath;
			string userId = path.Substring("/todos/".Length);
			byte[] encryptedData = new byte[request.ContentLength64];
			await request.InputStream.ReadAsync(encryptedData, 0, encryptedData.Length);
			string filePath = Path.Combine(STORAGE_DIR, $"todos_{userId}.dat");
			File.WriteAllBytes(filePath, encryptedData);
			Console.WriteLine($"   📁 Сохранены задачи пользователя {userId}: {encryptedData.Length} байт");
			response.StatusCode = 200;
			byte[] okBuffer = Encoding.UTF8.GetBytes("OK");
			response.ContentLength64 = okBuffer.Length;
			await response.OutputStream.WriteAsync(okBuffer, 0, okBuffer.Length);
		}
		private static async Task HandleLoadTodos(HttpListenerRequest request, HttpListenerResponse response)
		{
			string path = request.Url.AbsolutePath;
			string userId = path.Substring("/todos/".Length);
			string filePath = Path.Combine(STORAGE_DIR, $"todos_{userId}.dat");
			if (File.Exists(filePath))
			{
				byte[] encryptedData = File.ReadAllBytes(filePath);
				response.ContentType = "application/octet-stream";
				response.ContentLength64 = encryptedData.Length;
				await response.OutputStream.WriteAsync(encryptedData, 0, encryptedData.Length);
				Console.WriteLine($"   📂 Отданы задачи пользователя {userId}: {encryptedData.Length} байт");
			}
			else
			{
				response.ContentLength64 = 0;
				Console.WriteLine($"   ⚠️ Файл задач пользователя {userId} не найден");
			}
			response.StatusCode = 200;
		}
	}
}