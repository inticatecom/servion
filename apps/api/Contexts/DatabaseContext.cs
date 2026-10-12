using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Contexts;

/// <summary>
/// The database context made for accessing & interacting with the database in controllers & services.
/// </summary>
/// <param name="options">Options for the context.</param>
public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
{
    public DbSet<ServerModel> Servers { get; set; }

    public DbSet<UserModel> Users { get; set; }
}