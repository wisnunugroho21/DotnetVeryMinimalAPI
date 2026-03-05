using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Common.API.Result;
using VeryMinimalAPI.Common.Query;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.User;

public class GetAllUser : IEndpoint
{
    public record Request(int Skip = 0, int Take = 0, string Filters = "[]", string Sorts = "[]");
    
    public record Response(ListDataResult<Data.Types.User> Result);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/", Handle)
        .WithName(nameof(GetAllUser))
        .WithSummary("Create a new User");

    private static async Task<Ok<Response>> Handle([AsParameters] Request request, [FromServices] UserService service,
        CancellationToken cancellationToken)
    {
        var result = await service.GetAll(request.Skip, request.Take, 
            JsonConvert.DeserializeObject<List<Filter>>(
                !string.IsNullOrWhiteSpace(request.Filters) ? request.Filters : "[]"
            ) ?? [], 
            JsonConvert.DeserializeObject<List<Sort>>(
                !string.IsNullOrWhiteSpace(request.Sorts) ? request.Sorts : "[]"
            ) ?? [], 
            cancellationToken);
        return TypedResults.Ok(new Response(result));
    }
}