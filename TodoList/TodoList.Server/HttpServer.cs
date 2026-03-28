using System.Net;
using System.Text;
public class HttpServer
{
	private readonly HttpListener _listener;
	private readonly string _dataDirectory;
	private readonly string _profilesFilePath;
	public HttpServer(string prefix)
	{
		_listener = new HttpListener();
		_listener.Prefixes.Add(prefix);

		_dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServerData");
		_profilesFilePath = Path.Combine(_dataDirectory, "profiles.dat");

		if (!Directory.Exists(_dataDirectory))
		{
			Directory.CreateDirectory(_dataDirectory);
		}
	}
	public void Start()
	{
		_listener.Start();
		Task.Run(() => ListenAsync());
	}

	public void Stop()
	{
		_listener.Stop();
		_listener.Close();
	}
	private async Task ListenAsync()
	{
		while (_listener.IsListening)
		{
			try
			{
				var context = await _listener.GetContextAsync();
				_ = Task.Run(() => ProcessRequestAsync(context));
			}
			catch (HttpListenerException)
			{
				break;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
			}
		}
	}

	private async Task ProcessRequestAsync(HttpListenerContext context)
	{
		var request = context.Request;
		var response = context.Response;
		try
		{
			Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {request.HttpMethod} {request.Url?.AbsolutePath}");
			response.Headers.Add("Access-Control-Allow-Origin", "*");
			response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
			response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");
			if (request.HttpMethod == "OPTIONS")
			{
				response.StatusCode = (int)HttpStatusCode.OK;
				response.Close();
				return;
			}
			var path = request.Url?.AbsolutePath.Trim('/');
			switch (path)
			{
				case "profiles":
					await HandleProfilesAsync(request, response);
					break;

				case var p when p != null && p.StartsWith("todos/"):
					await HandleTodosAsync(request, response);
					break;

				default:
					response.StatusCode = (int)HttpStatusCode.NotFound;
					await WriteResponseAsync(response, "Endpoint not found");
					break;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка обработки: {ex.Message}");
			response.StatusCode = (int)HttpStatusCode.InternalServerError;
			await WriteResponseAsync(response, $"Server error: {ex.Message}");
		}
		finally
		{
			response.Close();
		}
	}
	private async Task HandleProfilesAsync(HttpListenerRequest request, HttpListenerResponse response)
	{
		if (request.HttpMethod == "GET")
		{
			if (File.Exists(_profilesFilePath))
			{
				var data = await File.ReadAllBytesAsync(_profilesFilePath);
				response.ContentType = "application/octet-stream";
				await response.OutputStream.WriteAsync(data, 0, data.Length);
				Console.WriteLine($"Отправлено профилей: {data.Length} байт");
			}
			else
			{
				response.StatusCode = (int)HttpStatusCode.NoContent;
				await WriteResponseAsync(response, "No profiles data");
			}
		}
		else if (request.HttpMethod == "POST")
		{
			using var ms = new MemoryStream();
			await request.InputStream.CopyToAsync(ms);
			var data = ms.ToArray();

			await File.WriteAllBytesAsync(_profilesFilePath, data);
			response.StatusCode = (int)HttpStatusCode.OK;
			await WriteResponseAsync(response, $"Profiles saved: {data.Length} bytes");
			Console.WriteLine($"Сохранено профилей: {data.Length} байт");
		}
		else
		{
			response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
			await WriteResponseAsync(response, "Method not allowed");
		}
	}
	private async Task HandleTodosAsync(HttpListenerRequest request, HttpListenerResponse response)
	{
		var path = request.Url?.AbsolutePath.Trim('/');
		var parts = path?.Split('/');

		if (parts == null || parts.Length < 2 || !Guid.TryParse(parts[1], out var userId))
		{
			response.StatusCode = (int)HttpStatusCode.BadRequest;
			await WriteResponseAsync(response, "Invalid userId");
			return;
		}
		var todosFilePath = Path.Combine(_dataDirectory, $"todos_{userId}.dat");
		if (request.HttpMethod == "GET")
		{
			if (File.Exists(todosFilePath))
			{
				var data = await File.ReadAllBytesAsync(todosFilePath);
				response.ContentType = "application/octet-stream";
				await response.OutputStream.WriteAsync(data, 0, data.Length);
				Console.WriteLine($"Отправлено задач для {userId}: {data.Length} байт");
			}
			else
			{
				response.StatusCode = (int)HttpStatusCode.NoContent;
				await WriteResponseAsync(response, "No todos data");
			}
		}
		else if (request.HttpMethod == "POST")
		{
			using var ms = new MemoryStream();
			await request.InputStream.CopyToAsync(ms);
			var data = ms.ToArray();

			await File.WriteAllBytesAsync(todosFilePath, data);
			response.StatusCode = (int)HttpStatusCode.OK;
			await WriteResponseAsync(response, $"Todos saved for {userId}: {data.Length} bytes");
			Console.WriteLine($"Сохранено задач для {userId}: {data.Length} байт");
		}
		else
		{
			response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
			await WriteResponseAsync(response, "Method not allowed");
		}
	}
	private async Task WriteResponseAsync(HttpListenerResponse response, string text)
	{
		var buffer = Encoding.UTF8.GetBytes(text);
		response.ContentType = "text/plain";
		response.ContentLength64 = buffer.Length;
		await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
	}
}