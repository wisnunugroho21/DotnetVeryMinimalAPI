using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Common.API.Result;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.User;

public class DeleteUser : IEndpoint
{
    public record Request(long Id);

    public record Response(ProcessResult Result);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapDelete("/{Id:long}", Handle)
        .WithName(nameof(DeleteUser))
        .WithSummary("Delete a User");

    private static async Task<Ok<Response>> Handle([AsParameters] Request request, [FromServices] UserService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Delete(request.Id, cancellationToken);
        return TypedResults.Ok(new Response(result));
    }
}