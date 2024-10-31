using MediatR;
using Sample.API.Common;
using SC4013.Application.TodoItems.Commands.Assign;
using SC4013.Application.TodoItems.Commands.Create;
using SC4013.Application.TodoItems.Commands.Delete;
using SC4013.Application.TodoItems.Commands.Update;
using SC4013.Application.TodoItems.Commands.UpdateDueDate;
using SC4013.Application.TodoItems.Queries;
using SC4013.Web.Common;

namespace SC4013.Web.Endpoints;

public class TodoItems : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetTodoItems)
            .MapPost(CreateTodoItem)
            .MapPut(AssignTodoItem, "Assign")
            .MapPut(UpdateTodoItem, "Update")
            .MapPut(UpdateDueDate, "UpdateDueDate")
            .MapDelete(DeleteTodoItem, "{id}");
    }
    
    private Task<List<TodoItemDto>> GetTodoItems(ISender sender, IHttpContextAccessor httpContextAccessor, [AsParameters] GetTodoItemsQuery query)
    {
        return sender.Send(query);
    }
    
    private Task<int> CreateTodoItem(ISender sender, CreateTodoItemCommand command)
    {
        return sender.Send(command);
    }
    
    private async Task<IResult> AssignTodoItem(ISender sender, AssignTodoItemCommand command)
    {
        await sender.Send(command);
        return Results.NoContent();
    }
    
    private async Task<IResult> UpdateTodoItem(ISender sender, UpdateTodoItemCommand command)
    {
        await sender.Send(command);
        return Results.NoContent();
    }
    
    private async Task<IResult> UpdateDueDate(ISender sender, UpdateDueDateCommand command)
    {
        await sender.Send(command);
        return Results.NoContent();
    }

    private async Task<IResult> DeleteTodoItem(ISender sender, int id)
    {
        await sender.Send(new DeleteTodoItemCommand(id));
        return Results.NoContent();
    }
        
}