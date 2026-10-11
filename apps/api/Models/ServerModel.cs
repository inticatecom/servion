namespace API.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Name), IsUnique = true)]
public class ServerModel
{
    [Key] public Guid Id { get; init; } = Guid.NewGuid();

    [MaxLength(30)] public required string Name { get; set; }
    public required string ContainerId { get; set; }
}