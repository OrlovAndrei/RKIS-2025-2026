using TodoList.Presentation.WebApi.DependencyInjection;

namespace TodoList.Presentation.WebApi;

public static class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddApplicationServices();

		// Добавляем сервисы для контроллеров
		builder.Services.AddControllers();

		builder.Services.AddLogging();

		var app = builder.Build();

		// Маппим контроллеры
		app.MapControllers();

		app.Run();
	}
}