using System.ComponentModel.DataAnnotations;
using TodoApp.API.Models;

namespace TodoApp.API.DTOs;

public class TodoCreateDto
{
    [Required, MaxLength(500)]
    public string Text { get; set; } = string.Empty;
}

public class TodoUpdateDto
{
    [Required, MaxLength(500)]
    public string Text { get; set; } = string.Empty;
}

public class TodoStatusUpdateDto
{
    [Required]
    public TodoStatus Status { get; set; }
}

public class TodoResponseDto
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public TodoStatus Status { get; set; }

    public DateTime LastUpdate { get; set; }
}