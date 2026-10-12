using System.ComponentModel.DataAnnotations;

namespace API.Models;

public abstract class IdentityModel
{
    [Key] public Guid Id = Guid.NewGuid();

    public DateTime UpdatedAt = DateTime.UtcNow;

    public DateTime CreatedAt = DateTime.UtcNow;
}