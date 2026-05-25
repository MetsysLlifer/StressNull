using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using StressNull.DTOs;

namespace StressNull.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private const string AuthTokenKey = "auth_token";

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        try
        {
            var token = await SecureStorage.Default.GetAsync(AuthTokenKey);
            return !string.IsNullOrEmpty(token);
        }
        catch
        {
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        SecureStorage.Default.Remove(AuthTokenKey);
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string?> LoginAsync(string username, string password)
    {
        try
        {
            var loginDto = new LoginDto { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                if (authResponse != null && !string.IsNullOrEmpty(authResponse.Token))
                {
                    await SecureStorage.Default.SetAsync(AuthTokenKey, authResponse.Token);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.Token);
                    return null; // Success
                }
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return !string.IsNullOrEmpty(errorContent) ? errorContent : "Login failed.";
        }
        catch (Exception ex)
        {
            return $"Network error: {ex.Message}. Make sure the backend API is running.";
        }
    }

    public async Task<string?> RegisterAsync(string username, string password)
    {
        try
        {
            var registerDto = new RegisterDto { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);

            if (response.IsSuccessStatusCode)
            {
                return null; // Success
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return !string.IsNullOrEmpty(errorContent) ? errorContent : "Registration failed.";
        }
        catch (Exception ex)
        {
            return $"Network error: {ex.Message}. Make sure the backend API is running.";
        }
    }

    public async Task<List<LeaderboardEntryDto>> GetLeaderboardAsync(string? gameMode = null)
    {
        try
        {
            var url = "api/leaderboard";
            if (!string.IsNullOrEmpty(gameMode) && gameMode != "All Modes")
            {
                url += $"?gameMode={gameMode}";
            }

            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<LeaderboardEntryDto>>() ?? new List<LeaderboardEntryDto>();
            }

            return new List<LeaderboardEntryDto>();
        }
        catch (Exception)
        {
            // Return empty list if server is unreachable
            return new List<LeaderboardEntryDto>();
        }
    }

    public async Task<bool> SubmitScoreAsync(int score, string gameMode)
    {
        try
        {
            var token = await SecureStorage.Default.GetAsync(AuthTokenKey);
            if (string.IsNullOrEmpty(token))
                return false;

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var dto = new ScoreSubmissionDto { Score = score, GameMode = gameMode };
            var response = await _httpClient.PostAsJsonAsync("api/leaderboard", dto);

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}