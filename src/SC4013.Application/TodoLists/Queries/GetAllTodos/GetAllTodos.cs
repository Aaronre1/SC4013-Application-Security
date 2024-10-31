using MediatR;
using Microsoft.EntityFrameworkCore;
using SC4013.Application.Common.Interfaces;
using SC4013.Application.Common.Security;

namespace SC4013.Application.TodoLists.Queries.GetAllTodos;

[Authorize]
public class GetAllTodosQuery: IRequest<IReadOnlyCollection<TodoListDto>>
{
    
}

public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, IReadOnlyCollection<TodoListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllTodosQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<TodoListDto>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.TodoLists.AsNoTracking()
            .Select(x => new TodoListDto
            {
                Id = x.Id,
                Title = x.Title,
                Items = x.Items.Select(i => new TodoItemDto
                {
                    Id = i.Id,
                    ListId = i.ListId,
                    Title = i.Title,
                    Done = i.Done,
                    Priority = (int)i.Priority,
                    Description = i.Description,
                    DueDate = i.DueDate
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}