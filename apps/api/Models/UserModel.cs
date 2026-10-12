using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class UserModel : IdentityModel
{
    [StringLength(30, MinimumLength = 3)] public required string Username { get; set; }

    [StringLength(30, MinimumLength = 3)] public required string Email { get; set; }

    [MaxLength(512)] public required string PasswordHash { get; set; }

    public ICollection<ServerModel> Servers { get; set; } = new List<ServerModel>();
}