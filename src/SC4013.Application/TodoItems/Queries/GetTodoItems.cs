using MediatR;
using Microsoft.EntityFrameworkCore;
using SC4013.Application.Common.Interfaces;
using SC4013.Application.Common.Security;

namespace SC4013.Application.TodoItems.Queries;

[Authorize]
public record GetTodoItemsQuery : IRequest<List<TodoItemDto>>
{
    public int ListId { get; set; }
}

public class GetTodoItemsQueryHandler : IRequestHandler<GetTodoItemsQuery, List<TodoItemDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTodoItemsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TodoItemDto>> Handle(GetTodoItemsQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.TodoItems
            .Where(x => x.ListId == request.ListId)
            .Select(x => new TodoItemDto
            {
                Id = x.Id,
                ListId = x.ListId,
                Title = x.Title,
                Description = x.Description,
                PriorityLevel = x.Priority,
                DueDate = x.DueDate,
                Done = x.Done,
                AssigneeEmail = x.AssigneeEmail,
                CreatedOn = x.CreatedOn,
                CreatedBy = x.CreatedBy,
                ModifiedOn = x.ModifiedOn,
                ModifiedBy = x.ModifiedBy
            })
            .ToListAsync(cancellationToken);
        
        return result;
    }
}