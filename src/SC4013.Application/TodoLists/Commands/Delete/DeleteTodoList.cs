using MediatR;
using SC4013.Application.Common.Interfaces;
using SC4013.Application.Common.Models;
using SC4013.Application.Common.Security;
using SC4013.Domain.Constants;

namespace SC4013.Application.TodoLists.Commands.Delete;

[Authorize(Roles = $"{Roles.Admin}")]
public record DeleteTodoListCommand(int Id) : IRequest<Result>;

public class DeleteTodoListCommandHandler : IRequestHandler<DeleteTodoListCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists.FindAsync([request.Id], cancellationToken: cancellationToken);
        
        if (entity == null)
        {
            return Result.Failure(new List<string> {"Todo list not found"});
        }
        
        _context.TodoLists.Remove(entity);
        
        await _context.SaveChangesAsync(cancellationToken); 
        
        return Result.Success();
    }
}