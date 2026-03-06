using VeryMinimalAPI.Common.Auth.Services;
using VeryMinimalAPI.Common.Services;
using VeryMinimalAPI.Data;
using VeryMinimalAPI.Data.Types;

namespace VeryMinimalAPI.Features.Services;

public class TodoService(AppDbContext context, ClaimService claim, ILogger<TodoService> logger) : CrudService<Todo>(context, claim, logger)
{
    
}