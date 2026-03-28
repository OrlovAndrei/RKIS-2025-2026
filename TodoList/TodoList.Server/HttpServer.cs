using System.Net;
using System.Text;
using System.Text.Json;
using TodoApp.Commands.Models;
using TodoList.Server.Models;
using TodoList.Server.Storage;
namespace TodoList.Server;
public class HttpServer
{
	private readonly HttpListener _listener;
	private readonly ServerStorageManager _storage;
	private readonly CancellationTokenSource _cts = new();
	private readonly JsonSerializerOptions _jsonOptions;

	public HttpServer(string prefix, ServerStorageManager storage)
	{
		_listener = new HttpListener();
		_listener.Prefixes.Add(prefix);
		_storage = storage;

		_jsonOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = true
		};
	}
	public void Start()
	{
		_listener.Start();
		Console.WriteLine($"Сервер запущен на {string.Join(", ", _listener.Prefixes)}");

		Task.Run(async () =>
		{
			while (!_cts.Token.IsCancellationRequested)
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
					Console.WriteLine($"Ошибка обработки запроса: {ex.Message}");
				}
			}
		});
	}
	public void Stop()
	{
		_cts.Cancel();
		_listener.Stop();
		_listener.Close();
		Console.WriteLine("Сервер остановлен");
	}
	private async Task ProcessRequestAsync(HttpListenerContext context)
	{
		var request = context.Request;
		var response = context.Response;
		try
		{
			Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {request.HttpMethod} {request.Url?.AbsolutePath}");

			response.ContentType = "application/json";
			response.Headers.Add("Access-Control-Allow-Origin", "*");
			response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
			response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

			if (request.HttpMethod == "OPTIONS")
			{
				response.StatusCode = (int)HttpStatusCode.OK;
				response.Close();
				return;
			}
			ApiResponse apiResponse;

			switch (request.Url?.AbsolutePath)
			{
				case "/api/profiles":
					apiResponse = await HandleProfilesRequest(request);
					break;

				case "/api/profiles/login":
					apiResponse = await HandleLoginRequest(request);
					break;

				case "/api/todos":
					apiResponse = await HandleTodosRequest(request);
					break;

				case "/api/sync":
					apiResponse = await HandleSyncRequest(request);
					break;

				case "/api/health":
					apiResponse = ApiResponse.Ok(null, "Server is healthy");
					break;

				default:
					response.StatusCode = (int)HttpStatusCode.NotFound;
					apiResponse = ApiResponse.Error("Endpoint not found");
					break;
			}

			var jsonResponse = JsonSerializer.Serialize(apiResponse, _jsonOptions);
			var buffer = Encoding.UTF8.GetBytes(jsonResponse);
			response.ContentLength64 = buffer.Length;
			await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
			response.Close();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Ошибка обработки запроса: {ex.Message}");
			response.StatusCode = (int)HttpStatusCode.InternalServerError;
			var errorResponse = ApiResponse.Error($"Internal server error: {ex.Message}");
			var jsonResponse = JsonSerializer.Serialize(errorResponse, _jsonOptions);
			var buffer = Encoding.UTF8.GetBytes(jsonResponse);
			response.ContentLength64 = buffer.Length;
			await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
			response.Close();
		}
	}
	private async Task<ApiResponse> HandleProfilesRequest(HttpListenerRequest request)
	{
		if (request.HttpMethod == "GET")
		{
			var profiles = _storage.GetAllProfiles();
			return ApiResponse.Ok(profiles);
		}

		if (request.HttpMethod == "POST")
		{
			var body = await ReadRequestBody(request);
			var profile = JsonSerializer.Deserialize<ProfileDto>(body, _jsonOptions);

			if (profile == null)
				return ApiResponse.Error("Invalid profile data");

			_storage.AddOrUpdateProfile(profile);
			return ApiResponse.Ok(profile, "Profile saved successfully");
		}

		return ApiResponse.Error("Method not allowed");
	}
	private async Task<ApiResponse> HandleLoginRequest(HttpListenerRequest request)
	{
		if (request.HttpMethod != "POST")
			return ApiResponse.Error("Method not allowed");

		var body = await ReadRequestBody(request);
		var loginData = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

		if (loginData == null || !loginData.TryGetValue("login", out var login) || !loginData.TryGetValue("password", out var password))
			return ApiResponse.Error("Login and password required");

		var profile = _storage.GetProfileByLogin(login);

		if (profile == null || profile.Password != password)
			return ApiResponse.Error("Invalid login or password");

		return ApiResponse.Ok(profile, "Login successful");
	}
	private async Task<ApiResponse> HandleTodosRequest(HttpListenerRequest request)
	{
		if (!request.QueryString.AllKeys.Contains("userId"))
			return ApiResponse.Error("UserId is required");

		if (!Guid.TryParse(request.QueryString["userId"], out var userId))
			return ApiResponse.Error("Invalid UserId format");

		if (request.HttpMethod == "GET")
		{
			var todos = _storage.GetTodos(userId);
			return ApiResponse.Ok(todos);
		}
		if (request.HttpMethod == "POST" || request.HttpMethod == "PUT")
		{
			var body = await ReadRequestBody(request);
			var todos = JsonSerializer.Deserialize<List<TodoItemDto>>(body, _jsonOptions);

			if (todos == null)
				return ApiResponse.Error("Invalid todos data");

			_storage.SaveTodos(userId, todos);
			return ApiResponse.Ok(null, "Todos saved successfully");
		}

		return ApiResponse.Error("Method not allowed");
	}
	private async Task<ApiResponse> HandleSyncRequest(HttpListenerRequest request)
	{
		if (request.HttpMethod != "POST")
			return ApiResponse.Error("Method not allowed");
		var body = await ReadRequestBody(request);
		var syncRequest = JsonSerializer.Deserialize<SyncRequest>(body, _jsonOptions);

		if (syncRequest == null)
			return ApiResponse.Error("Invalid sync request");
		var result = _storage.SyncUserData(syncRequest.UserId, syncRequest.Profiles, syncRequest.Todos);
		var serverProfiles = _storage.GetAllProfiles();
		var serverTodos = _storage.GetTodos(syncRequest.UserId);
		return ApiResponse.Ok(new
		{
			result.ProfilesSynced,
			result.TodosSynced,
			result.Message,
			Profiles = serverProfiles,
			Todos = serverTodos
		}, "Sync completed");
	}
	private async Task<string> ReadRequestBody(HttpListenerRequest request)
	{
		using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
		return await reader.ReadToEndAsync();
	}
}