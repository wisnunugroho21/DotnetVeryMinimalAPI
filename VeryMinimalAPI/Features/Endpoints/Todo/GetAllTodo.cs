using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VeryMinimalAPI.Common.API;
using VeryMinimalAPI.Common.API.Result;
using VeryMinimalAPI.Common.Query;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Features.Endpoints.Todo;

public class GetAllTodo : IEndpoint
{
    public record Request(int Skip = 0, int Take = 0, string Filters = "[]", string Sorts = "[]");
    
    public record Response(ListDataResult<Data.Types.Todo> Result);

    public static void Map(IEndpointRouteBuilder app) => app
        .MapGet("/", Handle)
        .WithName(nameof(GetAllTodo))
        .WithSummary("Create a new Todo");

    private static async Task<Results<Ok<Response>, InternalServerError<Response>>> Handle([AsParameters] Request request, [FromServices] TodoService service,
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
        var response = new Response(result);
        
        return result.Errors is not null && result.Errors.Any()
            ? TypedResults.Ok(response)
            : TypedResults.InternalServerError(response);
    }
}