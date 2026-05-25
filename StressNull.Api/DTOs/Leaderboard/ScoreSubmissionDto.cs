using System.ComponentModel.DataAnnotations;

namespace StressNull.Api.DTOs.Leaderboard;

public class ScoreSubmissionDto
{
    [Required]
    public int Score { get; set; }

    [Required]
    [MaxLength(50)]
    public required string GameMode { get; set; }
}