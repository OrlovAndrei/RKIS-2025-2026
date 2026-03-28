namespace TodoList.Server.Models;
public class TodoItemDto
{
	public string Text { get; set; } = string.Empty;
	public int Status { get; set; }
	public DateTime LastUpdate { get; set; }
}