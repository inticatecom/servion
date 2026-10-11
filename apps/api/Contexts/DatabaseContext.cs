using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Contexts;

public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<ServerModel> Servers { get; set; }
}