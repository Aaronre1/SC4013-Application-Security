using MediatR;
using SC4013.Application.TodoLists.Commands.Create;
using SC4013.Application.TodoLists.Commands.Delete;
using SC4013.Application.TodoLists.Commands.Update;
using SC4013.Application.TodoLists.Queries.GetAllTodos;
using SC4013.Web.Common;
using TodoListDto = SC4013.Application.TodoLists.Queries.GetAllTodos.TodoListDto;

namespace SC4013.Web.Endpoints;

public class TodoLists : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetAllTodos)
            .MapPost(CreateTodoList)
            .MapPut(UpdateTodoList, "Update")
            .MapDelete(DeleteTodoList, "{id}");
    }

    private Task<IReadOnlyCollection<TodoListDto>> GetAllTodos(ISender sender)
    {
        return sender.Send(new GetAllTodosQuery());
    }

    private Task<int> CreateTodoList(ISender sender, CreateTodoListCommand command)
    {
        return sender.Send(command);
    }

    private async Task<IResult> UpdateTodoList(ISender sender, UpdateTodoListCommand command)
    {
        await sender.Send(command);
        return Results.NoContent();
    }

    private async Task<IResult> DeleteTodoList(ISender sender, int id)
    {
        await sender.Send(new DeleteTodoListCommand(id));
        return Results.NoContent();
    }
}