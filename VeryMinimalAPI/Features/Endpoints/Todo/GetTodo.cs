using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.Todo;

public class GetTodo : IEndpoint
{
    public record Request(int Id);

    public record Response(Data.Types.Todo? Result);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/{Id:int}", Handle)
        .WithName(nameof(GetTodo))
        .WithSummary("Create a new Todo");

    private static async Task<Ok<Response>> Handle([AsParameters] Request request, [FromServices] TodoService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Get(request.Id, cancellationToken);
        return TypedResults.Ok(new Response(result));
    }
}