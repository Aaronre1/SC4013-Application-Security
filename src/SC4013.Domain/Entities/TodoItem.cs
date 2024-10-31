using SC4013.Domain.Common;
using SC4013.Domain.Enums;

namespace SC4013.Domain.Entities;

public class TodoItem : BaseEntity
{
    public int ListId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public PriorityLevel Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public bool Done { get; set; }
    public TodoList List { get; set; } = null!;
    public string? AssigneeEmail { get; set; }
}