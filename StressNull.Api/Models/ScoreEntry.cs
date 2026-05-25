using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StressNull.Api.Models;

public class ScoreEntry
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [Required]
    public int Score { get; set; }

    [Required]
    [MaxLength(50)]
    public required string GameMode { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}