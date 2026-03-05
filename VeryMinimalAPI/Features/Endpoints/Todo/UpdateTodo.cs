using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Common.API.Result;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.Todo;

public class UpdateTodo : IEndpoint
{
    public record Request(Data.Types.Todo Todo);

    public record Response(ProcessResult Result);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapPut("/", Handle)
        .WithName(nameof(UpdateTodo))
        .WithSummary("Create a new Todo");

    private static async Task<Ok<Response>> Handle([FromBody] Request request, [FromServices] TodoService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Update(request.Todo, cancellationToken);
        return TypedResults.Ok(new Response(result));
    }
}