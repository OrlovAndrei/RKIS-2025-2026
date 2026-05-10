using TodoList.Models;

namespace TodoList.Api.DTOs;

public sealed class TodoItemResponse
{
    public int Id { get; set; }
    public string Text { get; set; } = "";
    public TodoStatus Status { get; set; }
    public DateTime LastUpdate { get; set; }
}
