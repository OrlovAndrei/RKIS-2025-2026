using TodoList.Models;

namespace TodoList.Api.DTOs;

public sealed class SetStatusRequest
{
    public TodoStatus Status { get; set; }
}
