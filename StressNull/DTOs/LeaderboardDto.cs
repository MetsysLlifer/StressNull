namespace StressNull.DTOs;

public class ScoreSubmissionDto
{
    public int Score { get; set; }
    public string GameMode { get; set; } = string.Empty;
}

public class LeaderboardEntryDto
{
    public int Rank { get; set; }
    public string Username { get; set; } = string.Empty;
    public int Score { get; set; }
    public string GameMode { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
}