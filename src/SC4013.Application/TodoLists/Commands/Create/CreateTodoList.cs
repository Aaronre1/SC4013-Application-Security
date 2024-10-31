using MediatR;
using SC4013.Application.Common.Interfaces;
using SC4013.Application.Common.Security;
using SC4013.Domain.Constants;
using SC4013.Domain.Entities;

namespace SC4013.Application.TodoLists.Commands.Create;

[Authorize(Roles = $"{Roles.Admin}, {Roles.Manager}")]
public record CreateTodoListCommand : IRequest<int>
{
    public string? Title { get; set; }
}

public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoList
        {
            Title = request.Title
        };

        _context.TodoLists.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}