using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VeryMinimalAPI.Common.Auth.Services;
using VeryMinimalAPI.Common.Auth.Types;
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

        public void AddSecurity()
        {
            builder.Services.Configure<SecurityOptions>(builder.Configuration.GetSection("Security"));
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<IAuthorizationHandler, TokenHandlerService<AppDbContext>>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<ClaimService>();
            
            var provider = builder.Services.BuildServiceProvider();
            var securityOptions = provider.GetRequiredService<IOptions<SecurityOptions>>();
            
            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.ClaimsIssuer = securityOptions.Value.JwtIssuer;
                    opt.Audience = securityOptions.Value.JwtAudience;
                    
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityOptions.Value.JwtKey)),
                        ValidIssuer = securityOptions.Value.JwtIssuer,
                        ValidAudience = securityOptions.Value.JwtAudience,
                        
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorizationBuilder()
                .AddDefaultPolicy("default", opt => opt.Requirements.Add(new TokenRequirement()));

            builder.Services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(dp =>
                {
                    dp.AllowAnyHeader()
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .WithExposedHeaders("Content-Disposition");

                    if (securityOptions.Value.AllowedOrigin.Length > 0)
                    {
                        dp.WithOrigins(securityOptions.Value.AllowedOrigin);
                    }

                    else
                    {
                        dp.SetIsOriginAllowed(_ => true);
                    }
                });
            });
        }
        
        public void AddApplicationService()
        {   
            builder.Services.AddScoped<TodoService>();
            builder.Services.AddScoped<UserService>();
        }
    }
}