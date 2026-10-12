namespace API.Models;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Name), IsUnique = true)]
public class ServerModel : IdentityModel
{
    [StringLength(30, MinimumLength = 3)] public required string Name { get; set; }

    [MaxLength(64)] public required string ContainerId { get; set; }
}