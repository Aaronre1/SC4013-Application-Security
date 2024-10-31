using FluentValidation;
using Microsoft.EntityFrameworkCore;
using SC4013.Application.Common.Interfaces;

namespace SC4013.Application.TodoLists.Commands.Update;

public class UpdateTodoListCommandValidator : AbstractValidator<UpdateTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoListCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Title)
            .MaximumLength(200)
            .NotEmpty()
            .MustAsync(BeUniqueTitle)
            .WithMessage("The specified title already exists.");
    }

    private async Task<bool> BeUniqueTitle(UpdateTodoListCommand model, string title,
        CancellationToken cancellationToken)
    {
        return await _context.TodoLists
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Title != title, cancellationToken);
    }
}