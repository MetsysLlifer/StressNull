using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StressNull.Api.Data;
using StressNull.Api.DTOs.Leaderboard;
using StressNull.Api.Models;

namespace StressNull.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaderboardController : ControllerBase
{
    private readonly AppDbContext _context;

    public LeaderboardController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetLeaderboard([FromQuery] string? gameMode = null, [FromQuery] int limit = 50)
    {
        var query = _context.ScoreEntries
            .Include(s => s.User)
            .AsQueryable();

        if (!string.IsNullOrEmpty(gameMode))
        {
            query = query.Where(s => s.GameMode.ToLower() == gameMode.ToLower());
        }

        var topScores = await query
            .OrderByDescending(s => s.Score)
            .Take(limit)
            .Select(s => new LeaderboardEntryDto
            {
                Username = s.User!.Username,
                Score = s.Score,
                GameMode = s.GameMode,
                SubmittedAt = s.CreatedAt
            })
            .ToListAsync();

        // Assign ranks
        for (int i = 0; i < topScores.Count; i++)
        {
            topScores[i].Rank = i + 1;
        }

        return Ok(topScores);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> SubmitScore([FromBody] ScoreSubmissionDto dto)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized("Invalid user token.");
        }

        var existingEntry = await _context.ScoreEntries
            .FirstOrDefaultAsync(s => s.UserId == userId && s.GameMode.ToLower() == dto.GameMode.ToLower());

        if (existingEntry != null)
        {
            if (dto.Score > existingEntry.Score)
            {
                existingEntry.Score = dto.Score;
                existingEntry.CreatedAt = DateTime.UtcNow;
            }
        }
        else
        {
            var entry = new ScoreEntry
            {
                UserId = userId,
                Score = dto.Score,
                GameMode = dto.GameMode
            };
            _context.ScoreEntries.Add(entry);
        }

        await _context.SaveChangesAsync();

        return Ok("Brainrot score submitted successfully.");
    }
}