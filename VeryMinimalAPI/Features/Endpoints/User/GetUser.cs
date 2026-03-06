using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Common.API.Result;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.User;

public class GetUser : IEndpoint
{
    public record Request(long Id);

    public record Response(DataResult<Data.Types.User?> Result);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/{Id:long}", Handle)
        .WithName(nameof(GetUser))
        .WithSummary("Create a new User");

    private static async Task<Results<Ok<Response>, InternalServerError<Response>>> Handle([AsParameters] Request request, [FromServices] UserService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Get(request.Id, cancellationToken);
        var response = new Response(result);
        
        return result.Errors is not null && result.Errors.Any()
            ? TypedResults.InternalServerError(response)
            : TypedResults.Ok(response);
    }
}