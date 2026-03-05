using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Common.API.Result;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.Todo;

public class DeleteTodo : IEndpoint
{
    public record Request(long Id);

    public record Response(ProcessResult Result);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapDelete("/{Id:long}", Handle)
        .WithName(nameof(DeleteTodo))
        .WithSummary("Delete a Todo");

    private static async Task<Ok<Response>> Handle([AsParameters] Request request, [FromServices] TodoService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Delete(request.Id, cancellationToken);
        return TypedResults.Ok(new Response(result));
    }
}