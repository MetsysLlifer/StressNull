using StressNull.Services;
using StressNull.DTOs;
using System.Collections.ObjectModel;

namespace StressNull;

public partial class LeaderboardPage : ContentPage
{
    private readonly ApiService _apiService;
    public ObservableCollection<LeaderboardEntryDto> Scores { get; set; } = new();

    public LeaderboardPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        LeaderboardCollection.ItemsSource = Scores;
        ModePicker.SelectedIndex = 0;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadScores();
    }

    private async Task LoadScores()
    {
        string? filter = null;
        if (ModePicker.SelectedIndex > 0)
        {
            filter = ModePicker.SelectedItem.ToString();
        }

        var results = await _apiService.GetLeaderboardAsync(filter);
        Scores.Clear();
        foreach (var r in results)
        {
            Scores.Add(r);
        }
    }

    private async void OnRefreshing(object sender, EventArgs e)
    {
        await LoadScores();
        LeaderboardRefresh.IsRefreshing = false;
    }

    private async void OnModePickerSelectedIndexChanged(object sender, EventArgs e)
    {
        await LoadScores();
    }
}