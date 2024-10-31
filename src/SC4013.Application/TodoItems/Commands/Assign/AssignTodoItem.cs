using MediatR;
using SC4013.Application.Common.Interfaces;
using SC4013.Application.Common.Models;
using SC4013.Application.Common.Security;
using SC4013.Domain.Constants;

namespace SC4013.Application.TodoItems.Commands.Assign;

[Authorize(Roles = $"{Roles.Leader}, {Roles.Admin}, {Roles.Manager}")]
public class AssignTodoItemCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string? AssigneeEmail { get; set; }
}

public class AssignTodoCommandHandler : IRequestHandler<AssignTodoItemCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public AssignTodoCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result> Handle(AssignTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems.FindAsync([request.Id], cancellationToken: cancellationToken);
        
        if (entity == null)
        {
            return Result.Failure(new List<string> {"Todo item not found"});
        }
        
        entity.AssigneeEmail = request.AssigneeEmail;

        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}