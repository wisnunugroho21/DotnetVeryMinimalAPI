using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.Todo;

public class GetAllTodo : IEndpoint
{
    public record Response(IEnumerable<Data.Types.Todo> Result);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/", Handle)
        .WithName(nameof(GetAllTodo))
        .WithSummary("Create a new Todo");

    private static async Task<Ok<Response>> Handle([FromServices] TodoService service,
        CancellationToken cancellationToken)
    {
        var result = await service.GetAll(cancellationToken);
        return TypedResults.Ok(new Response(result));
    }
}