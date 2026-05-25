namespace StressNull.Api.DTOs.Leaderboard;

public class LeaderboardEntryDto
{
    public int Rank { get; set; }
    public required string Username { get; set; }
    public int Score { get; set; }
    public required string GameMode { get; set; }
    public DateTime SubmittedAt { get; set; }
}