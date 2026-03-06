using VeryMinimalAPI.Common.Auth.Services;
using VeryMinimalAPI.Common.Services;
using VeryMinimalAPI.Data;
using VeryMinimalAPI.Data.Types;

namespace VeryMinimalAPI.Features.Services;

public class UserService(AppDbContext context, ClaimService claim, ILogger<UserService> logger) : CrudService<User>(context, claim, logger)
{
    
}