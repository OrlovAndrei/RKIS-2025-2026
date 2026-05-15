using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Server
{
    class Program
    {
        private static readonly string _dataDirectory = "ServerData";

        static async Task Main(string[] args)
        {
            if (!Directory.Exists(_dataDirectory))
                Directory.CreateDirectory(_dataDirectory);

            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();

            Console.WriteLine("=== TodoList Server ===");
            Console.WriteLine($"Сервер запущен: http://localhost:5000/");
            Console.WriteLine("Нажмите Enter для остановки...");
            Console.WriteLine();

            var serverTask = Task.Run(async () =>
            {
                while (listener.IsListening)
                {
                    try
                    {
                        var context = await listener.GetContextAsync();
                        _ = HandleRequestAsync(context);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                }
            });

            Console.ReadLine();
            listener.Stop();
            Console.WriteLine("Сервер остановлен.");
        }

        private static async Task HandleRequestAsync(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            try
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] {request.HttpMethod} {request.Url?.AbsolutePath}");

                if (request.HttpMethod == "GET" && request.Url?.AbsolutePath == "/profiles")
                {
                    await HandleGetProfiles(response);
                }
                else if (request.HttpMethod == "POST" && request.Url?.AbsolutePath == "/profiles")
                {
                    await HandlePostProfiles(request, response);
                }
                else if (request.HttpMethod == "GET" && request.Url?.AbsolutePath.StartsWith("/todos/") == true)
                {
                    string userId = request.Url.AbsolutePath.Substring("/todos/".Length);
                    await HandleGetTodos(response, userId);
                }
                else if (request.HttpMethod == "POST" && request.Url?.AbsolutePath.StartsWith("/todos/") == true)
                {
                    string userId = request.Url.AbsolutePath.Substring("/todos/".Length);
                    await HandlePostTodos(request, response, userId);
                }
                else
                {
                    response.StatusCode = 404;
                    byte[] error = Encoding.UTF8.GetBytes("Not Found");
                    response.ContentLength64 = error.Length;
                    await response.OutputStream.WriteAsync(error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки: {ex.Message}");
                response.StatusCode = 500;
                byte[] error = Encoding.UTF8.GetBytes("Internal Server Error");
                response.ContentLength64 = error.Length;
                await response.OutputStream.WriteAsync(error);
            }
            finally
            {
                response.Close();
            }
        }

        private static async Task HandleGetProfiles(HttpListenerResponse response)
        {
            string filePath = Path.Combine(_dataDirectory, "profiles.dat");

            if (!File.Exists(filePath))
            {
                response.StatusCode = 404;
                byte[] notFound = Encoding.UTF8.GetBytes("Not Found");
                response.ContentLength64 = notFound.Length;
                await response.OutputStream.WriteAsync(notFound);
                return;
            }

            byte[] data = await File.ReadAllBytesAsync(filePath);
            response.ContentType = "application/octet-stream";
            response.ContentLength64 = data.Length;
            response.StatusCode = 200;
            await response.OutputStream.WriteAsync(data);
        }

        private static async Task HandlePostProfiles(HttpListenerRequest request, HttpListenerResponse response)
        {
            byte[] buffer = new byte[request.ContentLength64];
            await request.InputStream.ReadAsync(buffer, 0, buffer.Length);

            string filePath = Path.Combine(_dataDirectory, "profiles.dat");
            await File.WriteAllBytesAsync(filePath, buffer);

            response.StatusCode = 200;
            byte[] ok = Encoding.UTF8.GetBytes("OK");
            response.ContentLength64 = ok.Length;
            await response.OutputStream.WriteAsync(ok);
        }

        private static async Task HandleGetTodos(HttpListenerResponse response, string userId)
        {
            string filePath = Path.Combine(_dataDirectory, $"todos_{userId}.dat");

            if (!File.Exists(filePath))
            {
                response.StatusCode = 404;
                byte[] notFound = Encoding.UTF8.GetBytes("Not Found");
                response.ContentLength64 = notFound.Length;
                await response.OutputStream.WriteAsync(notFound);
                return;
            }

            byte[] data = await File.ReadAllBytesAsync(filePath);
            response.ContentType = "application/octet-stream";
            response.ContentLength64 = data.Length;
            response.StatusCode = 200;
            await response.OutputStream.WriteAsync(data);
        }

        private static async Task HandlePostTodos(HttpListenerRequest request, HttpListenerResponse response, string userId)
        {
            byte[] buffer = new byte[request.ContentLength64];
            await request.InputStream.ReadAsync(buffer, 0, buffer.Length);

            string filePath = Path.Combine(_dataDirectory, $"todos_{userId}.dat");
            await File.WriteAllBytesAsync(filePath, buffer);

            response.StatusCode = 200;
            byte[] ok = Encoding.UTF8.GetBytes("OK");
            response.ContentLength64 = ok.Length;
            await response.OutputStream.WriteAsync(ok);
        }
    }
}