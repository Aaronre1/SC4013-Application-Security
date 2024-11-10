using MediatR;
using Microsoft.EntityFrameworkCore;
using SC4013.Application.Common.Interfaces;
using SC4013.Application.Common.Security;

namespace SC4013.Application.TodoLists.Queries.GetTodoLists;

[Authorize]
public class GetTodoListsQuery : IRequest<List<TodoListDto>>
{
}

public class GetTodoListsQueryHandler : IRequestHandler<GetTodoListsQuery, List<TodoListDto>>
{
    private readonly IApplicationDbContext _context;

    public GetTodoListsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<TodoListDto>> Handle(GetTodoListsQuery request, CancellationToken cancellationToken)
    {
        var result = await _context.TodoLists
            .Select(x => new TodoListDto
            {
                Id = x.Id,
                Title = x.Title,
                CreatedOn = x.CreatedOn,
                CreatedBy = x.CreatedBy,
                ModifiedOn = x.ModifiedOn,
                ModifiedBy = x.ModifiedBy
            })
            .ToListAsync(cancellationToken);

        return result;
    }
}