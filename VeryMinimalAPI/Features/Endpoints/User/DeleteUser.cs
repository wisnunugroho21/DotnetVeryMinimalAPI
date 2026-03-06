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

    private static async Task<Results<Ok<Response>, InternalServerError<Response>>> Handle([AsParameters] Request request, [FromServices] UserService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Delete(request.Id, cancellationToken);
        var response = new Response(result);

        return result.IsSuccess ? TypedResults.Ok(response) : TypedResults.InternalServerError(response);
    }
}