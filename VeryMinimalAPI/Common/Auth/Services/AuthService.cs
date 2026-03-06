using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VeryMinimalAPI.Common.Auth.Types;
using VeryMinimalAPI.Data;
using VeryMinimalAPI.Data.Types;

namespace VeryMinimalAPI.Common.Auth.Services;

public class AuthService(IOptions<SecurityOptions> options, AppDbContext db)
{
    public async Task<string> Login(string username, string password, CancellationToken cancellationToken)
    {
        var user = await db.Users.SingleOrDefaultAsync(x => x.Username == username && x.Password == password, cancellationToken);
        return user is null ? string.Empty : GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(options.Value.JwtKey);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Claims = new Dictionary<string, object>() {
                { JwtRegisteredClaimNames.Name, user?.Username ?? "" },
                { JwtRegisteredClaimNames.Sub, user?.Username ?? "" }
            }
        };

        var token = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        var jws = jwtTokenHandler.WriteToken(token);

        return jws;
    }
}