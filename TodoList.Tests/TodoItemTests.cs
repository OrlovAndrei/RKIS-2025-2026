using System;
using TodoList;
using Xunit;

namespace TodoList.Tests;

public class TodoItemTests
{
    [Fact]
    public void Constructor_WithText_SetsDefaultStatusAndDate()
    {
        var text = "Test task";
        var item = new TodoItem(text);

        Assert.Equal(text, item.Text);
        Assert.Equal(TodoStatus.NotStarted, item.Status);
        Assert.True((DateTime.Now - item.LastUpdate).TotalSeconds < 1);
    }

    [Fact]
    public void Constructor_WithAllParameters_SetsProperties()
    {
        var text = "Test";
        var status = TodoStatus.InProgress;
        var date = new DateTime(2025, 3, 3, 12, 0, 0);

        var item = new TodoItem(text, status, date);

        Assert.Equal(text, item.Text);
        Assert.Equal(status, item.Status);
        Assert.Equal(date, item.LastUpdate);
    }

    [Fact]
    public void SetStatus_UpdatesStatusAndDate()
    {
        var item = new TodoItem("Task");
        var oldDate = item.LastUpdate;
        var newStatus = TodoStatus.Completed;

        item.SetStatus(newStatus);

        Assert.Equal(newStatus, item.Status);
        Assert.True(item.LastUpdate > oldDate);
    }

    [Fact]
    public void UpdateText_ChangesTextAndDate()
    {
        var item = new TodoItem("Old text");
        var oldDate = item.LastUpdate;
        var newText = "New text";

        item.UpdateText(newText);

        Assert.Equal(newText, item.Text);
        Assert.True(item.LastUpdate > oldDate);
    }

    [Fact]
    public void GetFullInfo_ReturnsFormattedString()
    {
        var item = new TodoItem("Task", TodoStatus.InProgress, new DateTime(2025, 3, 3, 14, 30, 0));
        var expected = $"Текст: Task\nСтатус: InProgress\nДата изменения: 03.03.2025 14:30";

        var info = item.GetFullInfo();

        Assert.Equal(expected, info);
    }
}