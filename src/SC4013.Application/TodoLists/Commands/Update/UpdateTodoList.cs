using MediatR;
using SC4013.Application.Common.Interfaces;
using SC4013.Application.Common.Models;
using SC4013.Application.Common.Security;
using SC4013.Domain.Constants;

namespace SC4013.Application.TodoLists.Commands.Update;

[Authorize(Roles = $"{Roles.Admin}, {Roles.Manager}")]
public class UpdateTodoListCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string? Title { get; set; }
}

public class UpdateTodoListCommandHandler : IRequestHandler<UpdateTodoListCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists.FindAsync([request.Id], cancellationToken: cancellationToken);

        if (entity == null)
        {
            return Result.Failure(new List<string> {"Todo list not found"});
        }

        entity.Title = request.Title;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}