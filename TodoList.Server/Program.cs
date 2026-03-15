using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Server
{
    internal class Program
    {
        private static readonly string _dataDirectory = "server_data";
        private static HttpListener _listener;

        static async Task Main(string[] args)
        {
            Console.WriteLine("TodoList Server");
            Console.WriteLine("================");

            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
                Console.WriteLine($"Создана папка: {_dataDirectory}");
            }

            _listener = new HttpListener();
            _listener.Prefixes.Add("http://localhost:5000/");
            _listener.Start();

            Console.WriteLine("Сервер запущен на http://localhost:5000/");
            Console.WriteLine("Ожидание запросов...");
            Console.WriteLine("Для остановки нажмите Ctrl+C\n");

            while (true)
            {
                try
                {
                    HttpListenerContext context = await _listener.GetContextAsync();
                    
                    _ = HandleRequestAsync(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при получении запроса: {ex.Message}");
                }
            }
        }

        private static async Task HandleRequestAsync(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string method = request.HttpMethod;
            string url = request.Url?.AbsolutePath ?? "/";

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {method} {url}");

            try
            {
                if (method == "GET" && url == "/profiles")
                {
                    await HandleGetProfiles(response);
                }
                else if (method == "POST" && url == "/profiles")
                {
                    await HandlePostProfiles(request, response);
                }
                else if (method == "GET" && url.StartsWith("/todos/"))
                {
                    await HandleGetTodos(url, response);
                }
                else if (method == "POST" && url.StartsWith("/todos/"))
                {
                    await HandlePostTodos(url, request, response);
                }
                else
                {
                    response.StatusCode = 404;
                    string errorText = "Endpoint not found";
                    byte[] errorBuffer = Encoding.UTF8.GetBytes(errorText);
                    response.ContentType = "text/plain; charset=utf-8";
                    response.ContentLength64 = errorBuffer.Length;
                    await response.OutputStream.WriteAsync(errorBuffer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки запроса: {ex.Message}");
                response.StatusCode = 500;
                string errorText = "Internal server error";
                byte[] errorBuffer = Encoding.UTF8.GetBytes(errorText);
                response.ContentType = "text/plain; charset=utf-8";
                response.ContentLength64 = errorBuffer.Length;
                await response.OutputStream.WriteAsync(errorBuffer);
            }
            finally
            {
                response.Close();
            }
        }

        private static Task HandleGetProfiles(HttpListenerResponse response)
        {
            string profilesPath = Path.Combine(_dataDirectory, "profiles.dat");
            
            if (!File.Exists(profilesPath))
            {
                response.StatusCode = 404;
                return Task.CompletedTask;
            }

            byte[] data = File.ReadAllBytes(profilesPath);
            response.ContentType = "application/octet-stream";
            response.ContentLength64 = data.Length;
            response.StatusCode = 200;
            return response.OutputStream.WriteAsync(data, 0, data.Length);
        }

        private static async Task HandlePostProfiles(HttpListenerRequest request, HttpListenerResponse response)
        {
            string profilesPath = Path.Combine(_dataDirectory, "profiles.dat");
            
            byte[] encryptedData = await ReadRequestBodyAsync(request);
            
            await File.WriteAllBytesAsync(profilesPath, encryptedData);
            
            Console.WriteLine($"  Сохранено {encryptedData.Length} байт (профили)");
            
            response.StatusCode = 200;
        }

        private static Task HandleGetTodos(string url, HttpListenerResponse response)
        {
            string userId = url.Substring("/todos/".Length);
            string todosPath = Path.Combine(_dataDirectory, $"todos_{userId}.dat");
            
            if (!File.Exists(todosPath))
            {
                response.StatusCode = 404;
                return Task.CompletedTask;
            }

            byte[] data = File.ReadAllBytes(todosPath);
            response.ContentType = "application/octet-stream";
            response.ContentLength64 = data.Length;
            response.StatusCode = 200;
            return response.OutputStream.WriteAsync(data, 0, data.Length);
        }

        private static async Task HandlePostTodos(string url, HttpListenerRequest request, HttpListenerResponse response)
        {
            string userId = url.Substring("/todos/".Length);
            string todosPath = Path.Combine(_dataDirectory, $"todos_{userId}.dat");
            
            byte[] encryptedData = await ReadRequestBodyAsync(request);
            
            await File.WriteAllBytesAsync(todosPath, encryptedData);
            
            Console.WriteLine($"  Сохранено {encryptedData.Length} байт (задачи пользователя {userId})");
            
            response.StatusCode = 200;
        }

        private static async Task<byte[]> ReadRequestBodyAsync(HttpListenerRequest request)
        {
            using var ms = new MemoryStream();
            await request.InputStream.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}