using TodoList.Models;

namespace TodoList.Api.DTOs;

public sealed class UpdateTodoRequest
{
    public string Text { get; set; } = "";
    public TodoStatus Status { get; set; }
}
