using TodoApp.Commands;
using TodoApp.Exceptions;
using Xunit;
using TodoListClass = TodoApp.Commands.TodoList;
namespace TodoList.Tests
{
	public class TodoListTests
	{
		[Fact]
		public void Constructor_WithItems_InitializesList()
		{
			var items = new List<TodoItem> { new TodoItem("Task 1") };

			var todoList = new TodoListClass(items);

			Assert.Equal(1, todoList.Count);
		}
		[Fact]
		public void Add_WhenItemAdded_RaisesEvents()
		{
			var todoList = new TodoListClass(new List<TodoItem>());
			var addedRaised = false;
			var listChangedRaised = false;
			var statusChangedRaised = false;

			todoList.OnTodoAdded += (list) => addedRaised = true;
			todoList.OnTodoListChanged += (list) => listChangedRaised = true;
			todoList.OnStatusChanged += (list) => statusChangedRaised = true;

			todoList.Add(new TodoItem("New task"));

			Assert.Equal(1, todoList.Count);
			Assert.True(addedRaised);
			Assert.True(listChangedRaised);
			Assert.True(statusChangedRaised);
		}
		[Fact]
		public void Delete_WithValidIndex_RemovesItemAndRaisesEvents()
		{
			var todoList = new TodoListClass(new List<TodoItem> { new TodoItem("Task 1") });
			var deletedRaised = false;
			var listChangedRaised = false;

			todoList.OnTodoDeleted += (list) => deletedRaised = true;
			todoList.OnTodoListChanged += (list) => listChangedRaised = true;

			todoList.Delete(0);

			Assert.Equal(0, todoList.Count);
			Assert.True(deletedRaised);
			Assert.True(listChangedRaised);
		}
		[Fact]
		public void Delete_WithInvalidIndex_DoesNotThrowAndHandlesException()
		{
			var todoList = new TodoListClass(new List<TodoItem>());
			var exception = Record.Exception(() => todoList.Delete(0));

			Assert.Null(exception);
			Assert.Equal(0, todoList.Count);
		}
		[Fact]
		public void SetStatus_WithValidIndex_UpdatesStatusAndRaisesEvents()
		{
			var todoList = new TodoListClass(new List<TodoItem> { new TodoItem("Task 1") });
			var updatedRaised = false;
			var listChangedRaised = false;

			todoList.OnTodoUpdated += (item) => updatedRaised = true;
			todoList.OnTodoListChanged += (list) => listChangedRaised = true;

			todoList.SetStatus(0, TodoStatus.InProgress);

			Assert.Equal(TodoStatus.InProgress, todoList[0].Status);
			Assert.True(updatedRaised);
			Assert.True(listChangedRaised);
		}
		[Fact]
		public void SetStatus_WithInvalidIndex_HandlesExceptionGracefully()
		{
			var todoList = new TodoListClass(new List<TodoItem>());
			var exception = Record.Exception(() => todoList.SetStatus(0, TodoStatus.InProgress));

			Assert.Null(exception);
		}
		[Fact]
		public void Update_WithExistingItem_UpdatesItemAndRaisesEvents()
		{
			var item = new TodoItem("Original task");
			var todoList = new TodoListClass(new List<TodoItem> { item });
			var updatedRaised = false;
			var newItem = new TodoItem("Updated task");

			todoList.OnTodoUpdated += (i) => updatedRaised = true;

			todoList.Update(newItem);

			Assert.True(updatedRaised);
		}
		[Fact]
		public void GetItem_WithValidIndex_ReturnsItem()
		{
			var expectedItem = new TodoItem("Task 1");
			var todoList = new TodoListClass(new List<TodoItem> { expectedItem });

			var item = todoList.GetItem(0);

			Assert.Equal(expectedItem, item);
		}
		[Fact]
		public void GetItem_WithInvalidIndex_ReturnsNull()
		{
			var todoList = new TodoListClass(new List<TodoItem>());

			var item = todoList.GetItem(0);

			Assert.Null(item);
		}
		[Fact]
		public void Indexer_ReturnsCorrectItem()
		{
			var expectedItem = new TodoItem("Task 1");
			var todoList = new TodoListClass(new List<TodoItem> { expectedItem });

			var item = todoList[0];

			Assert.Equal(expectedItem, item);
		}
		[Fact]
		public void GetEnumerator_ReturnsEnumeratorForItems()
		{
			var items = new List<TodoItem> { new TodoItem("Task 1"), new TodoItem("Task 2") };
			var todoList = new TodoListClass(items);

			var enumerator = todoList.GetEnumerator();

			int count = 0;
			while (enumerator.MoveNext()) count++;
			Assert.Equal(2, count);
		}
		[Fact]
		public void Count_WhenItemsAdded_ReturnsCorrectCount()
		{
			var todoList = new TodoListClass(new List<TodoItem>());

			todoList.Add(new TodoItem("Task 1"));
			todoList.Add(new TodoItem("Task 2"));

			Assert.Equal(2, todoList.Count);
		}
		[Fact]
		public void RequestSave_WhenCalledWithProfile_InvokesSaveRequestEvent()
		{
			var todoList = new TodoListClass(new List<TodoItem>());
			bool saveRequested = false;
			string savedFilePath = null;
			todoList.OnTodoListSaveRequested += (list, filePath) =>
			{
				saveRequested = true;
				savedFilePath = filePath;
			};

			todoList.RequestSave();

			Assert.True(saveRequested);
			Assert.Contains("todos_", savedFilePath);
		}
	}
}