namespace StressNull.Api.DTOs.Auth;

public class AuthResponseDto
{
    public required string Token { get; set; }
    public required string Username { get; set; }
    public int UserId { get; set; }
}