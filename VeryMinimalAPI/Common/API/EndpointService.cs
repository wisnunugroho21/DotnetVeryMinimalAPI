using VeryMinimalAPI.Features.Endpoints.Todo;

namespace VeryMinimalAPI.Common.API;

public static class EndpointService
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapTodoEndpoints();
    }

    extension(IEndpointRouteBuilder app)
    {
        private IEndpointRouteBuilder MapEndpoint<T>()
            where T : IEndpoint
        {
            T.Map(app);
            return app;
        }

        private void MapTodoEndpoints()
        {
            var endpoints = app
                .MapGroup("/todos")
                .WithTags("Todos");

            endpoints.MapEndpoint<GetAllTodo>()
                .MapEndpoint<GetTodo>()
                .MapEndpoint<CreateTodo>()
                .MapEndpoint<UpdateTodo>()
                .MapEndpoint<DeleteTodo>();
        }
    }
}