using VeryMinimalAPI.Common.Auth.Services;
using VeryMinimalAPI.Common.Services;
using VeryMinimalAPI.Data;
using VeryMinimalAPI.Data.Types;

namespace VeryMinimalAPI.Features.Services;

public class TodoService(AppDbContext context, ILogger<TodoService> logger) : CrudService<Todo>(context, logger)
{
    
}