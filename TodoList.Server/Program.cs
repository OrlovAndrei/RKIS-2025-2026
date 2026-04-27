using System.Net;
using System.Text;

namespace TodoList.Server;

class Program
{
    private static readonly string DataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "server_data");
    private static readonly string ProfilesFile = Path.Combine(DataDirectory, "profiles.dat");

    static async Task Main(string[] args)
    {
        Directory.CreateDirectory(DataDirectory);

        var listener = new HttpListener();
        listener.Prefixes.Add("http://localhost:5000/");
        listener.Start();
        Console.WriteLine("Сервер запущен на http://localhost:5000/");

        while (true)
        {
            var context = await listener.GetContextAsync();
            _ = HandleRequestAsync(context); 
        }
    }

    private static async Task HandleRequestAsync(HttpListenerContext context)
    {
        var request = context.Request;
        var response = context.Response;

        try
        {
            Console.WriteLine($"{request.HttpMethod} {request.Url}");

            var path = request.Url?.AbsolutePath.Trim('/').Split('/') ?? Array.Empty<string>();

            if (path.Length == 1 && path[0] == "profiles")
                await HandleProfiles(request, response);
            else if (path.Length == 2 && path[0] == "todos" && Guid.TryParse(path[1], out var userId))
                await HandleTodos(request, response, userId);
            else
            {
                response.StatusCode = 404;
                await WriteString(response, "Not Found");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            response.StatusCode = 500;
            await WriteString(response, "Internal Server Error");
        }
        finally
        {
            response.Close();
        }
    }

    private static async Task HandleProfiles(HttpListenerRequest request, HttpListenerResponse response)
    {
        if (request.HttpMethod == "POST")
        {
            byte[] body = await ReadBodyBytes(request);
            await File.WriteAllBytesAsync(ProfilesFile, body);
            response.StatusCode = 200;
            await WriteString(response, "OK");
        }
        else if (request.HttpMethod == "GET")
        {
            if (File.Exists(ProfilesFile))
            {
                byte[] data = await File.ReadAllBytesAsync(ProfilesFile);
                response.ContentType = "application/octet-stream";
                response.ContentLength64 = data.Length;
                await response.OutputStream.WriteAsync(data);
            }
            else
            {
                response.StatusCode = 404;
                await WriteString(response, "Profiles not found");
            }
        }
        else response.StatusCode = 405;
    }

    private static async Task HandleTodos(HttpListenerRequest request, HttpListenerResponse response, Guid userId)
    {
        string userFile = Path.Combine(DataDirectory, $"server_todos_{userId}.dat");

        if (request.HttpMethod == "POST")
        {
            byte[] body = await ReadBodyBytes(request);
            await File.WriteAllBytesAsync(userFile, body);
            response.StatusCode = 200;
            await WriteString(response, "OK");
        }
        else if (request.HttpMethod == "GET")
        {
            if (File.Exists(userFile))
            {
                byte[] data = await File.ReadAllBytesAsync(userFile);
                response.ContentType = "application/octet-stream";
                response.ContentLength64 = data.Length;
                await response.OutputStream.WriteAsync(data);
            }
            else
            {
                response.StatusCode = 404;
                await WriteString(response, "Todos not found");
            }
        }
        else response.StatusCode = 405;
    }

    private static async Task<byte[]> ReadBodyBytes(HttpListenerRequest request)
    {
        using var ms = new MemoryStream();
        await request.InputStream.CopyToAsync(ms);
        return ms.ToArray();
    }

    private static async Task WriteString(HttpListenerResponse response, string text)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(text);
        response.ContentType = "text/plain; charset=utf-8";
        response.ContentLength64 = buffer.Length;
        await response.OutputStream.WriteAsync(buffer);
    }
}