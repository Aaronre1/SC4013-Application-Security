using SC4013.Domain.Enums;

namespace SC4013.Application.TodoItems.Queries;

public class TodoItemDto
{
    public int Id { get; set; }
    public int ListId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public PriorityLevel Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public bool Done { get; set; }
    public string? AssigneeEmail { get; set; }
    
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset ModifiedOn { get; set; }
    public string? ModifiedBy { get; set; }
}