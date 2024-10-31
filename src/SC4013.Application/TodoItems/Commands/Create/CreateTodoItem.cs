using MediatR;
using SC4013.Application.Common.Interfaces;
using SC4013.Application.Common.Security;
using SC4013.Domain.Constants;
using SC4013.Domain.Entities;

namespace SC4013.Application.TodoItems.Commands.Create;

[Authorize(Roles = $"{Roles.Manager}, {Roles.Admin}")]
public record CreateTodoItemCommand : IRequest<int>
{
    public int ListId { get; init; }
    public string? Title { get; init; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        
        
        var entity = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Done = false
        };

        _context.TodoItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}