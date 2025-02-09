using Application.Commands;
using Application.Queries;
using MediatR;

namespace Presentation.Endpoints
{
    public static class TodoTasksEndpoints
    {
        public static RouteGroupBuilder MapTodoTasksEndpoints(this RouteGroupBuilder group) {

            #region POST: createTodoTask

            group.MapPost("/new", async (CreateTodoTaskCommand command, IMediator mediator) =>
            {
                try
                {
                    var id = await mediator.Send(command);
                    return Results.Created($"/{id}", id);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("PostNewTodoItem")
            .WithOpenApi();

            #endregion

            #region GET: getTodoTaskById

            group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                var task = await mediator.Send(new GetTodoTaskByIdQuery { Id = id });
                return task is null ? Results.NotFound($"Item Id={id} not found") : Results.Ok(task);
            })
            .WithName("GetTodoItemById")
            .WithOpenApi();

            #endregion

            #region POST: getTodoTasksPaginated

            group.MapPost("/all", async (GetTodoTasksQuery query, IMediator mediator) =>
            {
                var result = await mediator.Send(query);
                return Results.Ok(result);
            })
            .WithName("GetTodoItemsWithFilters")
            .WithOpenApi();

            #endregion

            #region DELETE: deleteTodoTaskById

            group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
            {
                try
                {
                    await mediator.Send(new DeleteTodoTaskCommand { Id = id });
                    return Results.NoContent();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("DeleteTodoItem")
            .WithOpenApi();

            #endregion

            #region PUT: updateTodoTaskById

            group.MapPut("/{id:guid}", async (Guid id, UpdateTodoTaskCommand command, IMediator mediator) =>
            {
                if (id != command.Id) return Results.BadRequest();
                try
                {
                    var updatedItem = await mediator.Send(command);
                    return updatedItem is null ? Results.NotFound($"Item Id={id} not found") : Results.Ok(updatedItem);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithName("UpdateTodoItem")
            .WithOpenApi();

            #endregion

            return group;
        }
    }
}
