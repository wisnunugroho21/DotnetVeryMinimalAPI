using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Common.Auth.Services;

namespace VeryMinimalAPI.Common.Auth.Endpoint;

public class Login : IEndpoint
{
    public record Request(string Username, string Password);

    public record Response(string Token);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapPost("/login", Handle)
        .WithName(nameof(Login))
        .WithSummary("Logs in an user");

    private static async Task<Results<Ok<Response>, UnauthorizedHttpResult>> Handle([FromBody] Request request, [FromServices] AuthService service,
        CancellationToken cancellationToken)
    {
        var result = await service.Login(request.Username, request.Password, cancellationToken);
        return TypedResults.Ok(new Response(result));
    }
}