using VeryMinimalAPI.Common.Auth.Services;
using VeryMinimalAPI.Common.Services;
using VeryMinimalAPI.Data;
using VeryMinimalAPI.Data.Types;

namespace VeryMinimalAPI.Features.Services;

public class UserService(AppDbContext context, ILogger<UserService> logger) : CrudService<User>(context, logger)
{
    
}