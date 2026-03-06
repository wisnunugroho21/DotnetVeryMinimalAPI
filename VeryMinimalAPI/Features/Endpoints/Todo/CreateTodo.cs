using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Common.API.Result;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.Todo;

public class CreateTodo : IEndpoint
{
    public record Request(Data.Types.Todo Todo);

    public record Response(ProcessResult Result);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/", Handle)
        .WithName(nameof(CreateTodo))
        .WithSummary("Create a new Todo");

    private static async Task<Results<Ok<Response>, InternalServerError<Response>>> Handle([FromBody] Request request, [FromServices] TodoService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Create(request.Todo, cancellationToken);
        var response = new Response(result);

        return result.IsSuccess ? TypedResults.Ok(response) : TypedResults.InternalServerError(response);
    }
}