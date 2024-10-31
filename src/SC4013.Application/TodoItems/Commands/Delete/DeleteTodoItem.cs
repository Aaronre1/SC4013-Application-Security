using MediatR;
using SC4013.Application.Common.Interfaces;
using SC4013.Application.Common.Models;
using SC4013.Application.Common.Security;
using SC4013.Domain.Constants;

namespace SC4013.Application.TodoItems.Commands.Delete;

[Authorize(Roles = $"{Roles.Admin}, {Roles.Manager}")]
public record DeleteTodoItemCommand(int Id) : IRequest<Result>;

public class DeleteTodoItemCommandHandler : IRequestHandler<DeleteTodoItemCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems.FindAsync([request.Id], cancellationToken: cancellationToken);
        
        if (entity == null)
        {
            return Result.Failure(new List<string> {"Todo item not found"});
        }
        
        _context.TodoItems.Remove(entity);
        
        await _context.SaveChangesAsync(cancellationToken); 
        
        return Result.Success();
    }
}