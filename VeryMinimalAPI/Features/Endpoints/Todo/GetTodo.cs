using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Common.API.Result;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.Todo;

public class GetTodo : IEndpoint
{
    public record Request(long Id);

    public record Response(DataResult<Data.Types.Todo?> Result);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/{Id:long}", Handle)
        .WithName(nameof(GetTodo))
        .WithSummary("Create a new Todo");

    private static async Task<Results<Ok<Response>, InternalServerError<Response>>> Handle([AsParameters] Request request, [FromServices] TodoService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Get(request.Id, cancellationToken);
        var response = new Response(result);

        return result.Errors is not null && result.Errors.Any()
            ? TypedResults.InternalServerError(response)
            : TypedResults.Ok(response);
    }
}