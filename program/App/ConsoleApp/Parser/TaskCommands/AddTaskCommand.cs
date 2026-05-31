using Application.Dto;
using Application.UseCase.TodoTaskUseCases;
using ConsoleApp.Adapters;
using ConsoleApp.Parser.Verb;

namespace ConsoleApp.Parser.TaskCommands;

internal static class AddTaskCommand
{
    private static readonly InputAdapter _input = new();

    public static async Task ExecuteAsync(TaskAdd t)
    {
        var name = string.IsNullOrWhiteSpace(t.Name)
            ? _input.GetShortText("Введите название задачи: ")
            : t.Name!;

        var createDto = new TodoTaskDto.TodoTaskCreateDto(
            State: t.StateId.HasValue ? Domain.Entities.TaskEntity.TaskState.ListState.GetById(t.StateId.Value) : null,
            Priority: t.PriorityLevel.HasValue ? Domain.Entities.TaskEntity.TaskPriority.ListPriority.GetByLevel(t.PriorityLevel.Value) : null,
            UserContext: Launch.UserContext,
            Name: name,
            Description: t.Description,
            Deadline: t.Deadline
        );

        var useCase = new AddNewTaskUseCase(
            repository: Launch.TodoTaskRepository,
            userContext: Launch.UserContext,
            taskCreate: createDto
        );

        await Launch.CommandManager.ExecuteCommandAsync(useCase);
    }
}
