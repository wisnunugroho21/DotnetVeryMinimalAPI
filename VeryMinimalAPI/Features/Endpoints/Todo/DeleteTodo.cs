using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.Todo;

public class DeleteTodo : IEndpoint
{
    public record Request(int Id);

    public record Response(IEnumerable<string> Messages);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapDelete("/{Id:int}", Handle)
        .WithName(nameof(DeleteTodo))
        .WithSummary("Delete a Todo");

    private static async Task<Ok<Response>> Handle([AsParameters] Request request, [FromServices] TodoService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Delete(request.Id, cancellationToken);
        return TypedResults.Ok(new Response(result));
    }
}