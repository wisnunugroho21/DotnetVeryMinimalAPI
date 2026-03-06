using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Common.API.Result;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.User;

public class UpdateUser : IEndpoint
{
    public record Request(Data.Types.User User);

    public record Response(ProcessResult Result);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapPut("/", Handle)
        .WithName(nameof(UpdateUser))
        .WithSummary("Create a new User");

    private static async Task<Results<Ok<Response>, InternalServerError<Response>>> Handle([FromBody] Request request, [FromServices] UserService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Update(request.User, cancellationToken);
        var response = new Response(result);

        return result.IsSuccess ? TypedResults.Ok(response) : TypedResults.InternalServerError(response);
    }
}