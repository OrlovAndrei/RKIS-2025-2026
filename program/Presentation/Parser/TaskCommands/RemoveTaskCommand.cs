using Application.Dto;
using Application.Specifications.Criteria;
using Application.UseCase.TodoTaskUseCases;
using Application.UseCase.TodoTaskUseCases.Query;
using Presentation.Output.Implementation;
using Presentation.Parser.Verb;

namespace Presentation.Parser.TaskCommands;

internal static class RemoveTaskCommand
{
    public static async Task ExecuteAsync(TaskRemove t)
    {
        if (t.TaskId.HasValue)
        {
            // Удаление по ID
            var useCase = new DeletionTaskUseCase(
                repository: Launch.TodoTaskRepository,
                idTodoTask: t.TaskId.Value
            );

            await Launch.CommandManager.ExecuteCommandAsync(useCase);
        }
        else
        {
            // Удаление по критериям поиска
            var searchDto = new TodoTaskDto.TodoTaskSearchDto(
                UserContext: Launch.UserContext,
                Name: t.Name,
                Description: t.Description,
                DeadlineFrom: t.DeadlineFrom,
                DeadlineTo: t.DeadlineTo,
                SearchType: SearchTypes.Equals
            );

            var findUseCase = new FindTasksUseCase(repository: Launch.TodoTaskRepository, searchDto: searchDto);
            var tasksToDelete = await findUseCase.Execute();

            if (!tasksToDelete.Any())
            {
                WriteToConsole.ColorMessage("Задачи не найдены.");
                return;
            }

            foreach (var task in tasksToDelete)
            {
                var deleteUseCase = new DeletionTaskUseCase(
                    repository: Launch.TodoTaskRepository,
                    idTodoTask: task.TaskId
                );

                await Launch.CommandManager.ExecuteCommandAsync(deleteUseCase);
            }
        }
    }
}
