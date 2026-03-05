using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VeryMinimalAPI.Common.Auth.Services;
using VeryMinimalAPI.Data;
using VeryMinimalAPI.Features.Services;

namespace VeryMinimalAPI.Common.Services;

public static class ApplicationService
{
    extension(WebApplicationBuilder builder)
    {
        public void AddDatabase()
        {
            builder.Services.AddDbContext<AppDbContext>(opt => 
                opt.UseSqlServer(builder.Configuration.GetConnectionString("LocalDB")));
        }

        public void AddAuthentication()
        {
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();
            
            builder.Services.AddScoped<AuthService>();
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
        }
        
        public void AddApplicationService()
        {   
            builder.Services.AddScoped<TodoService>();
            builder.Services.AddScoped<UserService>();
        }
    }
}