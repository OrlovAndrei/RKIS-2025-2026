using System.Net;
using System.Text;

namespace TodoList.Server
{
    class Program
    {
        private const string BaseUrl = "http://localhost:5000/";
        private static readonly string DataDirectory = Path.Combine(
            Directory.GetCurrentDirectory(), "server_data");

        static async Task Main(string[] args)
        {
            Console.WriteLine("TodoList HTTP Server");
            Console.WriteLine("====================");
            Console.WriteLine($"Сервер запущен на: {BaseUrl}");
            Console.WriteLine("Для остановки нажмите Ctrl+C\n");

            if (!Directory.Exists(DataDirectory))
            {
                Directory.CreateDirectory(DataDirectory);
                Console.WriteLine($"Создана директория для данных: {DataDirectory}");
            }

            var listener = new HttpListener();
            listener.Prefixes.Add(BaseUrl);
            listener.Start();

            Console.WriteLine("Сервер готов к приёму запросов...\n");

            try
            {
                while (true)
                {
                    var context = await listener.GetContextAsync();
                    _ = Task.Run(() => HandleRequestAsync(context));
                }
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine($"Ошибка сервера: {ex.Message}");
            }
            finally
            {
                listener.Stop();
                Console.WriteLine("Сервер остановлен");
            }
        }

        private static async Task HandleRequestAsync(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {request.HttpMethod} {request.Url?.AbsolutePath}");

            try
            {
                if (request.HttpMethod == "POST" && request.Url?.AbsolutePath == "/profiles")
                {
                    await HandleSaveProfilesAsync(request, response);
                }
                else if (request.HttpMethod == "GET" && request.Url?.AbsolutePath == "/profiles")
                {
                    await HandleLoadProfilesAsync(request, response);
                }
                else if (request.HttpMethod == "POST" && request.Url?.AbsolutePath.StartsWith("/todos/") == true)
                {
                    await HandleSaveTodosAsync(request, response);
                }
                else if (request.HttpMethod == "GET" && request.Url?.AbsolutePath.StartsWith("/todos/") == true)
                {
                    await HandleLoadTodosAsync(request, response);
                }
                else
                {
                    response.StatusCode = 404;
                    byte[] buffer = Encoding.UTF8.GetBytes("{\"error\": \"Not Found\"}");
                    response.ContentType = "application/json";
                    response.ContentLength64 = buffer.Length;
                    await response.OutputStream.WriteAsync(buffer);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки запроса: {ex.Message}");
                response.StatusCode = 500;
                byte[] buffer = Encoding.UTF8.GetBytes("{\"error\": \"Internal Server Error\"}");
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer);
            }
            finally
            {
                response.Close();
            }
        }

        private static async Task HandleSaveProfilesAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            byte[] encryptedData;
            using (var ms = new MemoryStream())
            {
                await request.InputStream.CopyToAsync(ms);
                encryptedData = ms.ToArray();
            }

            if (encryptedData.Length == 0)
            {
                response.StatusCode = 400;
                byte[] buffer = Encoding.UTF8.GetBytes("{\"error\": \"Empty data\"}");
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer);
                return;
            }

            string filePath = Path.Combine(DataDirectory, "server_profiles.dat");
            await File.WriteAllBytesAsync(filePath, encryptedData);

            Console.WriteLine($"Сохранены профили: {encryptedData.Length} байт");

            response.StatusCode = 200;
            byte[] successBuffer = Encoding.UTF8.GetBytes("{\"status\": \"ok\"}");
            response.ContentType = "application/json";
            response.ContentLength64 = successBuffer.Length;
            await response.OutputStream.WriteAsync(successBuffer);
        }

        private static async Task HandleLoadProfilesAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            string filePath = Path.Combine(DataDirectory, "server_profiles.dat");

            if (!File.Exists(filePath))
            {
                byte[] emptyBuffer = Array.Empty<byte>();
                response.ContentType = "application/octet-stream";
                response.ContentLength64 = emptyBuffer.Length;
                await response.OutputStream.WriteAsync(emptyBuffer);
                return;
            }

            byte[] encryptedData = await File.ReadAllBytesAsync(filePath);

            Console.WriteLine($"Загружены профили: {encryptedData.Length} байт");

            response.ContentType = "application/octet-stream";
            response.ContentLength64 = encryptedData.Length;
            await response.OutputStream.WriteAsync(encryptedData);
        }

        private static async Task HandleSaveTodosAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            string path = request.Url!.AbsolutePath;
            string userId = path.Substring("/todos/".Length);

            if (string.IsNullOrEmpty(userId))
            {
                response.StatusCode = 400;
                byte[] buffer = Encoding.UTF8.GetBytes("{\"error\": \"Invalid userId\"}");
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer);
                return;
            }

            byte[] encryptedData;
            using (var ms = new MemoryStream())
            {
                await request.InputStream.CopyToAsync(ms);
                encryptedData = ms.ToArray();
            }

            if (encryptedData.Length == 0)
            {
                response.StatusCode = 400;
                byte[] buffer = Encoding.UTF8.GetBytes("{\"error\": \"Empty data\"}");
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer);
                return;
            }

            string filePath = Path.Combine(DataDirectory, $"server_todos_{userId}.dat");
            await File.WriteAllBytesAsync(filePath, encryptedData);

            Console.WriteLine($"Сохранены задачи для пользователя {userId}: {encryptedData.Length} байт");

            response.StatusCode = 200;
            byte[] successBuffer = Encoding.UTF8.GetBytes("{\"status\": \"ok\"}");
            response.ContentType = "application/json";
            response.ContentLength64 = successBuffer.Length;
            await response.OutputStream.WriteAsync(successBuffer);
        }

        private static async Task HandleLoadTodosAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            string path = request.Url!.AbsolutePath;
            string userId = path.Substring("/todos/".Length);

            if (string.IsNullOrEmpty(userId))
            {
                response.StatusCode = 400;
                byte[] buffer = Encoding.UTF8.GetBytes("{\"error\": \"Invalid userId\"}");
                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer);
                return;
            }

            string filePath = Path.Combine(DataDirectory, $"server_todos_{userId}.dat");

            if (!File.Exists(filePath))
            {
                byte[] emptyBuffer = Array.Empty<byte>();
                response.ContentType = "application/octet-stream";
                response.ContentLength64 = emptyBuffer.Length;
                await response.OutputStream.WriteAsync(emptyBuffer);
                return;
            }

            byte[] encryptedData = await File.ReadAllBytesAsync(filePath);

            Console.WriteLine($"Загружены задачи для пользователя {userId}: {encryptedData.Length} байт");

            response.ContentType = "application/octet-stream";
            response.ContentLength64 = encryptedData.Length;
            await response.OutputStream.WriteAsync(encryptedData);
        }
    }
}