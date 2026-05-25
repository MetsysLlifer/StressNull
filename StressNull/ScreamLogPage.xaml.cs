using StressNull.Services;

namespace StressNull;

public partial class ScreamLogPage : ContentPage
{
    private bool _isScreaming = false;
    private int _currentDb = 0;
    private int _maxDb = 0;
    private readonly Random _random = new();
    private readonly ApiService _apiService;

    public ScreamLogPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void OnScreamPressed(object sender, EventArgs e)
    {
        _isScreaming = true;
        _maxDb = 0;
        ResultLabel.Text = "LISTENING...";
        InstructionsLabel.IsVisible = false;
        
        await ScreamSimulationLoop();
    }

    private async void OnScreamReleased(object sender, EventArgs e)
    {
        _isScreaming = false;
        
        string stressLevel;
        if (_maxDb < 40) stressLevel = "Level: Dead Inside";
        else if (_maxDb < 70) stressLevel = "Level: Minor Inconvenience";
        else if (_maxDb < 95) stressLevel = "Level: Losing My Mind";
        else stressLevel = "Level: PURE CHAOS";

        ResultLabel.Text = $"LOGGED. {stressLevel} ({_maxDb} dB max)";
        DecibelLabel.Text = "0 dB";
        MeterFrame.BackgroundColor = Color.FromArgb("#333333");
        MeterFrame.Scale = 1.0;
        InstructionsLabel.IsVisible = true;
        InstructionsLabel.Text = "Log another scream?";

        if (_maxDb > 0)
        {
            bool isLoggedIn = await _apiService.IsLoggedInAsync();
            if (isLoggedIn)
            {
                await _apiService.SubmitScoreAsync(_maxDb, "Scream");
            }
        }
    }

    private async Task ScreamSimulationLoop()
    {
        // Simulates audio decibel reading changing rapidly
        while (_isScreaming)
        {
            // Randomly jump up and down, simulating a scream building up
            int targetDb = _random.Next(30, 110);
            
            // Smoothing the transition slightly
            _currentDb = (_currentDb + targetDb) / 2;

            if (_currentDb > _maxDb)
                _maxDb = _currentDb;

            DecibelLabel.Text = $"{_currentDb} dB";

            // Visual feedback based on intensity
            if (_currentDb > 90)
            {
                MeterFrame.BackgroundColor = Color.FromArgb("#FF0000");
                await MeterFrame.ScaleTo(1.2, 50);
            }
            else if (_currentDb > 60)
            {
                MeterFrame.BackgroundColor = Color.FromArgb("#FF8C00");
                await MeterFrame.ScaleTo(1.1, 50);
            }
            else
            {
                MeterFrame.BackgroundColor = Color.FromArgb("#008000");
                await MeterFrame.ScaleTo(1.0, 50);
            }

            await Task.Delay(50);
        }
    }
}