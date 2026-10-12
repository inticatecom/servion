namespace API.DTO;

public static class UserDTO
{
    public record Create(string Username, string Email, string Password);
}