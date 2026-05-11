using System.Net;
using TodoList.Server;
Console.WriteLine("=== TodoList Server ===");
Console.WriteLine("Запуск сервера на http://localhost:5000/");
var server = new HttpServer("http://localhost:5000/");
server.Start();
Console.WriteLine("Сервер запущен. Нажмите Ctrl+C для остановки.");
Console.WriteLine("Доступные эндпоинты:");
Console.WriteLine("  POST /profiles");
Console.WriteLine("  GET  /profiles");
Console.WriteLine("  POST /todos/{userId}");
Console.WriteLine("  GET  /todos/{userId}");
var cts = new CancellationTokenSource();
Console.CancelKeyPress += (sender, e) =>
{
	e.Cancel = true;
	cts.Cancel();
	server.Stop();
	Console.WriteLine("Сервер остановлен");
};
await Task.Delay(-1, cts.Token);