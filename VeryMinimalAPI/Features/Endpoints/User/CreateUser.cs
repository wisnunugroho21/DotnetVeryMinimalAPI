using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Common.API.Result;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.User;

public class CreateUser : IEndpoint
{
    public record Request(Data.Types.User User);

    public record Response(ProcessResult Result);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/", Handle)
        .WithName(nameof(CreateUser))
        .WithSummary("Create a new User");

    private static async Task<Results<Ok<Response>, InternalServerError<Response>>> Handle([FromBody] Request request, [FromServices] UserService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Create(request.User, cancellationToken);
        var response = new Response(result);

        return result.IsSuccess ? TypedResults.Ok(response) : TypedResults.InternalServerError(response);
    }
}