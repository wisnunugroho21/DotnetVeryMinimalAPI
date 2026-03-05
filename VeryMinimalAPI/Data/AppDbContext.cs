using Microsoft.EntityFrameworkCore;
using VeryMinimalAPI.Data.Types;

namespace VeryMinimalAPI.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos => Set<Todo>();

    public DbSet<User> Users => Set<User>();
}