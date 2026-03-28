using TodoList.Server;
using TodoList.Server.Storage;

Console.WriteLine("=== TodoList Server ===");
Console.WriteLine("Запуск сервера...");

var storage = new ServerStorageManager("ServerData");
var server = new HttpServer("http://localhost:8080/", storage);

Console.CancelKeyPress += (sender, e) =>
{
	e.Cancel = true;
	server.Stop();
	Environment.Exit(0);
};

server.Start();
Console.WriteLine("Сервер готов к работе. Нажмите Ctrl+C для остановки.");
Console.WriteLine("Доступные эндпоинты:");
Console.WriteLine("  GET  /api/profiles");
Console.WriteLine("  POST /api/profiles");
Console.WriteLine("  POST /api/profiles/login");
Console.WriteLine("  GET  /api/todos?userId={id}");
Console.WriteLine("  POST /api/todos?userId={id}");
Console.WriteLine("  POST /api/sync");
Console.WriteLine("  GET  /api/health");

await Task.Delay(-1);