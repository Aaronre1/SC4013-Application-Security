using SC4013.Domain.Common;

namespace SC4013.Domain.Entities;

public class TodoList : BaseEntity
{
    public string? Title { get; set; }
    public IList<TodoItem> Items { get; private set; } = new List<TodoItem>();
}