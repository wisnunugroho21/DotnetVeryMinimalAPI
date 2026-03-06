using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Common.API.Result;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.Todo;

public class GetTodo : IEndpoint
{
    public record Request(long Id);

    public record OkResponse(Data.Types.Todo? Data);
    
    public record ServerErrorResponse(IEnumerable<string>? Messages);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/{Id:long}", Handle)
        .WithName(nameof(GetTodo))
        .WithSummary("Create a new Todo");

    private static async Task<Results<Ok<OkResponse>, InternalServerError<ServerErrorResponse>>> Handle([AsParameters] Request request, [FromServices] TodoService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Get(request.Id, cancellationToken);

        return result.Errors is not null && result.Errors.Any()
            ? TypedResults.InternalServerError(new ServerErrorResponse(result.Errors))
            : TypedResults.Ok(new OkResponse(result.Data));
    }
}