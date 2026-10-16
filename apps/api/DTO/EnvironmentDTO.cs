using System.ComponentModel.DataAnnotations;
using API.Models;

namespace API.DTO;

public static class EnvironmentDto
{
    public record Create([Required] EnvironmentModel Data);

    public record Delete([Required] Guid Id);
}