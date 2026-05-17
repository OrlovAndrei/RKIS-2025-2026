using System.Net;
using System.Text;

namespace TodoList.Server;

internal class Program
{
    private static readonly string DataDirectory = "server_data";
    private static readonly object _lock = new object();

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== TodoList Server ===");
        Console.WriteLine("Практическую работу 5 (РКИС, 2 семестр) сделали: Фоменко и Мартиросьян");

        // Создаём папку для данных, если её нет
        if (!Directory.Exists(DataDirectory))
            Directory.CreateDirectory(DataDirectory);

        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:5000/");
        listener.Start();
        Console.WriteLine("Сервер запущен на http://localhost:5000/");
        Console.WriteLine("Ожидание запросов... (нажмите Ctrl+C для остановки)");

        while (true)
        {
            var context = await listener.GetContextAsync();
            _ = Task.Run(() => ProcessRequestAsync(context));
        }
    }

    private static async Task ProcessRequestAsync(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        try
        {
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {request.HttpMethod} {request.Url?.AbsolutePath}");

            if (request.HttpMethod == "POST" && request.Url?.AbsolutePath == "/profiles")
            {
                await SaveProfilesAsync(request, response);
            }
            else if (request.HttpMethod == "GET" && request.Url?.AbsolutePath == "/profiles")
            {
                await LoadProfilesAsync(response);
            }
            else if (request.HttpMethod == "POST" && request.Url?.AbsolutePath.StartsWith("/todos/") == true)
            {
                var userId = request.Url.AbsolutePath.Replace("/todos/", "");
                if (Guid.TryParse(userId, out var _))
                    await SaveTodosAsync(userId, request, response);
                else
                    SendErrorResponse(response, 400, "Invalid user ID");
            }
            else if (request.HttpMethod == "GET" && request.Url?.AbsolutePath.StartsWith("/todos/") == true)
            {
                var userId = request.Url.AbsolutePath.Replace("/todos/", "");
                if (Guid.TryParse(userId, out var _))
                    await LoadTodosAsync(userId, response);
                else
                    SendErrorResponse(response, 400, "Invalid user ID");
            }
            else
            {
                SendErrorResponse(response, 404, "Not found");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка обработки запроса: {ex.Message}");
            SendErrorResponse(response, 500, "Internal server error");
        }
    }

    private static async Task SaveProfilesAsync(HttpListenerRequest request, HttpListenerResponse response)
    {
        var filePath = Path.Combine(DataDirectory, "server_profiles.dat");
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await request.InputStream.CopyToAsync(fileStream);
        SendResponse(response, 200, "OK");
        Console.WriteLine("Профили сохранены");
    }

    private static async Task LoadProfilesAsync(HttpListenerResponse response)
    {
        var filePath = Path.Combine(DataDirectory, "server_profiles.dat");
        if (!File.Exists(filePath))
        {
            SendResponse(response, 200, "");
            return;
        }

        var data = await File.ReadAllBytesAsync(filePath);
        response.ContentType = "application/octet-stream";
        response.ContentLength64 = data.Length;
        await response.OutputStream.WriteAsync(data);
        response.StatusCode = 200;
        Console.WriteLine("Профили отправлены");
    }

    private static async Task SaveTodosAsync(string userId, HttpListenerRequest request, HttpListenerResponse response)
    {
        var filePath = Path.Combine(DataDirectory, $"server_todos_{userId}.dat");
        using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        await request.InputStream.CopyToAsync(fileStream);
        SendResponse(response, 200, "OK");
        Console.WriteLine($"Задачи пользователя {userId} сохранены");
    }

    private static async Task LoadTodosAsync(string userId, HttpListenerResponse response)
    {
        var filePath = Path.Combine(DataDirectory, $"server_todos_{userId}.dat");
        if (!File.Exists(filePath))
        {
            SendResponse(response, 200, "");
            return;
        }

        var data = await File.ReadAllBytesAsync(filePath);
        response.ContentType = "application/octet-stream";
        response.ContentLength64 = data.Length;
        await response.OutputStream.WriteAsync(data);
        response.StatusCode = 200;
        Console.WriteLine($"Задачи пользователя {userId} отправлены");
    }

    private static void SendResponse(HttpListenerResponse response, int statusCode, string message)
    {
        response.StatusCode = statusCode;
        var buffer = Encoding.UTF8.GetBytes(message);
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }

    private static void SendErrorResponse(HttpListenerResponse response, int statusCode, string message)
    {
        response.StatusCode = statusCode;
        var buffer = Encoding.UTF8.GetBytes(message);
        response.ContentLength64 = buffer.Length;
        response.OutputStream.Write(buffer, 0, buffer.Length);
        response.OutputStream.Close();
    }
}