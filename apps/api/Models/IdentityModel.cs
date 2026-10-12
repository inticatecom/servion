using System.ComponentModel.DataAnnotations;

namespace API.Models;

/// <summary>
/// A base identity model including a primary identifier & timestamps for operations/
/// </summary>
public abstract class IdentityModel
{
    /// <summary>
    /// The ID of the model.
    /// </summary>
    [Key] public Guid Id = Guid.NewGuid();

    /// <summary>
    /// The last time the model was updated at.
    /// </summary>
    public DateTime UpdatedAt = DateTime.UtcNow;

    /// <summary>
    /// The time the model was created at.
    /// </summary>
    public DateTime CreatedAt = DateTime.UtcNow;
}