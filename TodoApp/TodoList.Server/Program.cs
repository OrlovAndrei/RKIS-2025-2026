using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Server
{
    class Program
    {
        private const string Prefix = "http://localhost:5000/";
        private const string DataDirectory = "server_data";

        static async Task Main()
        {
            Directory.CreateDirectory(DataDirectory);

            using var listener = new HttpListener();
            listener.Prefixes.Add(Prefix);
            listener.Start();

            Console.WriteLine($"TodoList.Server слушает {Prefix}");

            while (true)
            {
                var context = await listener.GetContextAsync();
                _ = Task.Run(() => HandleRequestAsync(context));
            }
        }

        private static async Task HandleRequestAsync(HttpListenerContext context)
        {
            try
            {
                string method = context.Request.HttpMethod;
                string path = context.Request.Url?.AbsolutePath.Trim('/') ?? string.Empty;

                if (path == "profiles" && method == "POST")
                {
                    await SaveRequestBodyAsync(context, GetProfilesPath());
                    await WriteTextAsync(context, HttpStatusCode.OK, "OK");
                    return;
                }

                if (path == "profiles" && method == "GET")
                {
                    await WriteFileAsync(context, GetProfilesPath());
                    return;
                }

                if (path.StartsWith("todos/", StringComparison.OrdinalIgnoreCase))
                {
                    string userId = path.Substring("todos/".Length);
                    if (!Guid.TryParse(userId, out Guid parsedUserId))
                    {
                        await WriteTextAsync(context, HttpStatusCode.BadRequest, "Invalid user id");
                        return;
                    }

                    string todoPath = GetTodosPath(parsedUserId);
                    if (method == "POST")
                    {
                        await SaveRequestBodyAsync(context, todoPath);
                        await WriteTextAsync(context, HttpStatusCode.OK, "OK");
                        return;
                    }

                    if (method == "GET")
                    {
                        await WriteFileAsync(context, todoPath);
                        return;
                    }
                }

                await WriteTextAsync(context, HttpStatusCode.NotFound, "Not found");
            }
            catch (Exception ex)
            {
                await WriteTextAsync(context, HttpStatusCode.InternalServerError, ex.Message);
            }
            finally
            {
                context.Response.Close();
            }
        }

        private static async Task SaveRequestBodyAsync(HttpListenerContext context, string path)
        {
            using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
            await context.Request.InputStream.CopyToAsync(fileStream);
        }

        private static async Task WriteFileAsync(HttpListenerContext context, string path)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.ContentType = "application/octet-stream";

            if (!File.Exists(path))
            {
                return;
            }

            using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            await fileStream.CopyToAsync(context.Response.OutputStream);
        }

        private static async Task WriteTextAsync(HttpListenerContext context, HttpStatusCode statusCode, string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "text/plain; charset=utf-8";
            await context.Response.OutputStream.WriteAsync(bytes, 0, bytes.Length);
        }

        private static string GetProfilesPath()
        {
            return Path.Combine(DataDirectory, "server_profiles.dat");
        }

        private static string GetTodosPath(Guid userId)
        {
            return Path.Combine(DataDirectory, $"server_todos_{userId}.dat");
        }
    }
}
