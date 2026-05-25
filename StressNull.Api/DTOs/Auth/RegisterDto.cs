using System.ComponentModel.DataAnnotations;

namespace StressNull.Api.DTOs.Auth;

public class RegisterDto
{
    [Required]
    [MaxLength(50)]
    public required string Username { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public required string Password { get; set; }
}