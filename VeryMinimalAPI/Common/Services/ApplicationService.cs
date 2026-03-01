using Microsoft.EntityFrameworkCore;
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
        
        public void AddApplicationService()
        {
            builder.Services.AddScoped<TodoService>();
        }
    }
}