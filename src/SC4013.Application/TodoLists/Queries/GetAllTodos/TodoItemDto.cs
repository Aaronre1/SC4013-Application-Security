namespace SC4013.Application.TodoLists.Queries.GetAllTodos;

public class TodoItemDto
{
    public int Id { get; set; }
    public int ListId { get; set; }
    public string? Title { get; set; }
    public bool Done { get; set; }
    public int Priority { get; set; }
    public string? Description { get; set; }
    public string? AssigneeEmail { get; set; }
    public DateTimeOffset? DueDate { get; set; }
}