using Infrastructure.EfRepository;
using Microsoft.EntityFrameworkCore;
using TodoList.Database;
using TodoList.Infrastructure;
using TodoList.Interfaces;
using TodoList.Interfaces.Repositories;
using TodoList.UseCases.ProfileUseCases;
using TodoList.UseCases.ProfileUseCases.Query;
using TodoList.UseCases.TaskUseCases;
using TodoList.UseCases.TaskUseCases.Query;

namespace TodoList.Presentation.WebApi.DependencyInjection;

public static partial class DependencyInjectionConfig
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IClock, Clock>();
        services.AddScoped<ICurrentProfile, CurrentProfile>();
        services.AddScoped<IHasher, Hasher>();
        services.AddScoped<IControllerUndoRedo, ManagerUndoRedo>();
        
        services.AddScoped<IConnectionStrategy, SqliteStrategyDi>();
        services.AddScoped<ApplicationContext>();
        services.AddScoped<DbContext, ApplicationContext>();

        services.AddScoped<IProfileRepositories, EfProfileRepository>();
        services.AddScoped<ITaskItemRepositories, EfTodoTaskRepository>();
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();

        services.AddScoped<GetAllProfileUseCase>();
        services.AddScoped<AddProfileUseCase>();
        services.AddScoped<ChangeProfileUseCase>();
        services.AddScoped<DeleteProfileUseCase>();
        services.AddScoped<UpdateProfileUseCase>();

        services.AddScoped<GetAllTaskUseCase>();
        services.AddScoped<AddTaskUseCase>();
        services.AddScoped<DeleteTaskUseCase>();
        services.AddScoped<UpdateTaskUseCase>();
        return services;
    }
}