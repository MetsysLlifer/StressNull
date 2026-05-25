using System.ComponentModel.DataAnnotations;

namespace StressNull.Api.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public required string Username { get; set; }

    [Required]
    public required string PasswordHash { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property for related scores
    public ICollection<ScoreEntry> Scores { get; set; } = new List<ScoreEntry>();
}