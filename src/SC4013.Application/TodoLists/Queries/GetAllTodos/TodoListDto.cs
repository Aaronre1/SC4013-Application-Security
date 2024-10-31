namespace SC4013.Application.TodoLists.Queries.GetAllTodos;

public class TodoListDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public IReadOnlyCollection<TodoItemDto> Items { get; set; } = Array.Empty<TodoItemDto>();
}