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

    public DbSet<EnvironmentModel> Environments { get; set; }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IdentityModel>())
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
            }
        }
    }
}