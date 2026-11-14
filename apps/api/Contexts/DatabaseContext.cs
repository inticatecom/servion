using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Contexts;

/// <summary>
///     The database context made for accessing & interacting with the database in controllers & services.
/// </summary>
/// <param name="options">Options for the context.</param>
public sealed class DatabaseContext(
    DbContextOptions<DatabaseContext> options
) : IdentityDbContext<UserModel, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<ServerModel> Servers { get; set; }

    public DbSet<UserModel> Users { get; set; }

    public DbSet<EnvironmentModel> Environments { get; set; }

    public DbSet<ContainerModel> Containers { get; set; }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        UpdateTimestamps();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<IdentityModel>())
            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = now;
    }
}