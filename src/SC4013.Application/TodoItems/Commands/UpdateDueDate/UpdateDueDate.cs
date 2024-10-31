using MediatR;
using SC4013.Application.Common.Interfaces;
using SC4013.Application.Common.Models;
using SC4013.Application.Common.Security;
using SC4013.Domain.Constants;

namespace SC4013.Application.TodoItems.Commands.UpdateDueDate;

[Authorize(Roles = $"{Roles.Admin}, {Roles.Manager}")]
public class UpdateDueDateCommand : IRequest<Result>
{
    public int Id { get; set; }
    public DateTime? DueDate { get; set; }
}

public class UpdateDueDateCommandHandler : IRequestHandler<UpdateDueDateCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateDueDateCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Result> Handle(UpdateDueDateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems.FindAsync([request.Id], cancellationToken: cancellationToken);

        if (entity == null)
        {
            return Result.Failure(new List<string> {"Todo item not found"});
        }
        
        entity.DueDate = request.DueDate;
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}