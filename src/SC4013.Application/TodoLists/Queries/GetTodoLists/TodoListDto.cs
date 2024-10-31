namespace SC4013.Application.TodoLists.Queries.GetTodoLists;

public class TodoListDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset ModifiedOn { get; set; }
    public string? ModifiedBy { get; set; }
}