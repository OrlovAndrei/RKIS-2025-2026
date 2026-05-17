using System;
using System.Linq;
using TodoList;
using Xunit;

namespace TodoList.Tests;

public class TodoListTests
{
    [Fact]
    public void Add_Item_AddsToListAndRaisesEvent()
    {
        var list = new TodoList();
        var item = new TodoItem("Test");
        TodoItem eventItem = null;
        list.OnTodoAdded += (i) => eventItem = i;

        list.Add(item);

        Assert.Contains(item, list.items);
        Assert.Equal(item, eventItem);
    }

    [Fact]
    public void Delete_ExistingIndex_RemovesItemAndRaisesEvent()
    {
        var list = new TodoList();
        var item = new TodoItem("Task");
        list.Add(item);
        TodoItem deletedItem = null;
        list.OnTodoDeleted += (i) => deletedItem = i;

        list.Delete(0);

        Assert.Empty(list.items);
        Assert.Equal(item, deletedItem);
    }

    [Fact]
    public void Delete_InvalidIndex_ThrowsArgumentOutOfRangeException()
    {
        var list = new TodoList();
        list.Add(new TodoItem("Task"));

        Assert.Throws<ArgumentOutOfRangeException>(() => list.Delete(5));
    }

    [Fact]
    public void Insert_AtIndex_AddsItemAndRaisesEvent()
    {
        var list = new TodoList();
        list.Add(new TodoItem("First"));
        var newItem = new TodoItem("Inserted");
        TodoItem addedItem = null;
        list.OnTodoAdded += (i) => addedItem = i;

        list.Insert(0, newItem);

        Assert.Equal(newItem, list.items[0]);
        Assert.Equal(2, list.items.Count);
        Assert.Equal(newItem, addedItem);
    }

    [Fact]
    public void Remove_ExistingItem_RemovesAndRaisesEvent()
    {
        var list = new TodoList();
        var item = new TodoItem("ToRemove");
        list.Add(item);
        TodoItem deletedItem = null;
        list.OnTodoDeleted += (i) => deletedItem = i;

        list.Remove(item);

        Assert.DoesNotContain(item, list.items);
        Assert.Equal(item, deletedItem);
    }

    [Fact]
    public void Remove_NonExistingItem_DoesNothingAndNoEvent()
    {
        var list = new TodoList();
        var item = new TodoItem("NotInList");
        bool eventRaised = false;
        list.OnTodoDeleted += (i) => eventRaised = true;

        list.Remove(item);

        Assert.False(eventRaised);
    }

    [Fact]
    public void GetItem_ValidIndex_ReturnsItem()
    {
        var list = new TodoList();
        var item = new TodoItem("Task");
        list.Add(item);

        var result = list.GetItem(0);

        Assert.Equal(item, result);
    }

    [Fact]
    public void GetItem_InvalidIndex_ThrowsArgumentOutOfRangeException()
    {
        var list = new TodoList();

        Assert.Throws<ArgumentOutOfRangeException>(() => list.GetItem(0));
    }

    [Fact]
    public void View_DoesNotThrow_WhenCalled()
    {
        var list = new TodoList();
        list.Add(new TodoItem("Task"));

        var exception = Record.Exception(() => list.View(true, true, true));
        Assert.Null(exception);
    }

    [Fact]
    public void NotifyItemUpdated_RaisesEvent()
    {
        var list = new TodoList();
        var item = new TodoItem("Task");
        TodoItem eventItem = null;
        list.OnTodoUpdated += (i) => eventItem = i;

        list.NotifyItemUpdated(item);

        Assert.Equal(item, eventItem);
    }

    [Fact]
    public void NotifyStatusChanged_RaisesEvent()
    {
        var list = new TodoList();
        var item = new TodoItem("Task");
        TodoItem eventItem = null;
        list.OnStatusChanged += (i) => eventItem = i;

        list.NotifyStatusChanged(item);

        Assert.Equal(item, eventItem);
    }
}