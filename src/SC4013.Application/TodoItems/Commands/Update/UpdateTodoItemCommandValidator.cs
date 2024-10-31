using FluentValidation;

namespace SC4013.Application.TodoItems.Commands.Update;

public class UpdateTodoItemCommandValidator : AbstractValidator<UpdateTodoItemCommand>
{
    public UpdateTodoItemCommandValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(200);

        When(x => x.Title != null, () =>
        {
            RuleFor(x => x.Title)
                .NotEmpty();
        });
        
        
    }
}