using MediatR;
using SC4013.Application.Common.Interfaces;
using SC4013.Application.Common.Models;
using SC4013.Domain.Enums;

namespace SC4013.Application.TodoItems.Commands.Update;

public class UpdateTodoItemCommand : IRequest<Result>
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public bool? Done { get; set; }
    public PriorityLevel? PriorityLevel { get; set; }
    public string? Description { get; set; }
}

public class UpdateTodoItemCommandHandler : IRequestHandler<UpdateTodoItemCommand, Result>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(UpdateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems.FindAsync([request.Id], cancellationToken: cancellationToken);

        if (entity == null)
        {
            return Result.Failure(new List<string> {"Todo item not found"});
        }

        entity.Title = request.Title ?? entity.Title;
        entity.Done = request.Done ?? entity.Done;
        entity.Priority = request.PriorityLevel ?? entity.Priority;
        entity.Description = request.Description ?? entity.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}