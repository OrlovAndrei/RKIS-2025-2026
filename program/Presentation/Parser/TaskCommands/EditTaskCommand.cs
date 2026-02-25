using Application.Dto;
using Application.Specifications.Criteria;
using Application.UseCase.TodoTaskUseCases;
using Application.UseCase.TodoTaskUseCases.Query;
using Domain.Entities.TaskEntity;
using Presentation.Output.Implementation;
using Presentation.Parser.Verb;

namespace Presentation.Parser.TaskCommands;

internal static class EditTaskCommand
{
    public static async Task ExecuteAsync(TaskEdit t)
    {
        // Поиск задачи для редактирования
        var searchDto = new TodoTaskDto.TodoTaskSearchDto(
            UserContext: Launch.UserContext,
            TaskId: t.TaskIdSearch,
            StateId: t.StateIdSearch,
            PriorityLevelFrom: t.PriorityLevelFromSearch,
            PriorityLevelTo: t.PriorityLevelToSearch,
            Name: t.NameSearch,
            Description: t.DescriptionSearch,
            CreatedAtFrom: t.CreatedAtFromSearch,
            CreatedAtTo: t.CreatedAtToSearch,
            DeadlineFrom: t.DeadlineFromSearch,
            DeadlineTo: t.DeadlineToSearch,
            SearchType: SearchTypes.Equals
        );

        var findUseCase = new FindTasksUseCase(repository: Launch.TodoTaskRepository, searchDto: searchDto);
        var tasksToEdit = await findUseCase.Execute();

        if (!tasksToEdit.Any())
        {
            WriteToConsole.ColorMessage("Задачи для редактирования не найдены.");
            return;
        }

        if (tasksToEdit.Count() > 1)
        {
            WriteToConsole.ColorMessage("Найдено более одной задачи. Пожалуйста, уточните критерии поиска.");
            TaskPrinter.PrintTasks(tasksToEdit);
            return;
        }

        var taskToEdit = tasksToEdit.First();

        // Создание DTO для обновления с новыми значениями
        var updateDto = new TodoTaskDto.TodoTaskUpdateDto(
            TaskId: taskToEdit.TaskId,
            State: t.StateId.HasValue
                ? TaskState.ListState.GetById(t.StateId.Value)
                : TaskState.Uncertain,
            Priority: t.PriorityLevel.HasValue
                ? TaskPriority.ListPriority.GetByLevel(t.PriorityLevel.Value)
                : TaskPriority.Medium,
            Name: string.IsNullOrWhiteSpace(t.Name) ? taskToEdit.Name : t.Name,
            Description: t.Description ?? taskToEdit.Description,
            Deadline: t.Deadline ?? taskToEdit.Deadline
        );

        var updateUseCase = new UpdateTaskUseCase(
            repository: Launch.TodoTaskRepository,
            updateDto: updateDto
        );

        await Launch.CommandManager.ExecuteCommandAsync(updateUseCase);
    }
}
