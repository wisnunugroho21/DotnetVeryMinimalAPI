using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.Todo;

public class CreateTodo : IEndpoint
{
    public record Request(Data.Types.Todo Todo);

    public record Response(IEnumerable<string> Messages);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/", Handle)
        .WithName(nameof(CreateTodo))
        .WithSummary("Create a new Todo");

    private static async Task<Ok<Response>> Handle([FromBody] Request request, [FromServices] TodoService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Create(request.Todo, cancellationToken);
        return TypedResults.Ok(new Response(result));
    }
}