using Plugin.Maui.Audio;
using StressNull.Services;

namespace StressNull;

public partial class PandaPage : ContentPage
{
    private double _stressLevel = 0;
    private int _secondsSurvived = 0;
    private bool _isGameOver = false;
    private readonly IDispatcherTimer _gameTimer;
    private readonly Random _random = new();
    private readonly ApiService _apiService;

    public PandaPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        
        _gameTimer = Dispatcher.CreateTimer();
        _gameTimer.Interval = TimeSpan.FromMilliseconds(100);
        _gameTimer.Tick += OnGameTick;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        StartGame();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _gameTimer.Stop();
    }

    private void StartGame()
    {
        _stressLevel = 0;
        _secondsSurvived = 0;
        _isGameOver = false;
        StressBar.Progress = 0;
        StressBar.ProgressColor = Colors.Green;
        PandaImage.Source = "meme_panda.svg";
        StatusLabel.Text = "PANDA IS CALM";
        _gameTimer.Start();
    }

    private void OnGameTick(object? sender, EventArgs e)
    {
        if (_isGameOver) return;

        // Increase stress over time (faster as time goes on)
        double increment = 0.005 + (_secondsSurvived * 0.0005);
        _stressLevel += increment;
        StressBar.Progress = Math.Min(1.0, _stressLevel);

        // Update UI based on stress
        UpdatePandaState();

        if (_stressLevel >= 1.0)
        {
            GameOver();
        }
        // Update timer display (every 10 ticks = 1 second)

        _secondsSurvived = (int)(_secondsSurvived + 1);
        if (_secondsSurvived % 10 == 0)
        {
            TimeLabel.Text = $"SURVIVED: {_secondsSurvived / 10}s";
        }
    }

    private void UpdatePandaState()
    {
        if (_stressLevel < 0.3)
        {
            PandaImage.Source = "meme_panda_stress_0.svg";
            StatusLabel.Text = "PANDA IS CALM";
            StressBar.ProgressColor = Colors.Green;
        }
        else if (_stressLevel < 0.6)
        {
            PandaImage.Source = "meme_panda_stress_1.svg";
            StatusLabel.Text = "PANDA IS WORRIED";
            StressBar.ProgressColor = Colors.Yellow;
        }
        else if (_stressLevel < 0.8)
        {
            PandaImage.Source = "meme_panda_stress_2.svg";
            StatusLabel.Text = "PANDA IS SWEATING";
            StressBar.ProgressColor = Colors.Orange;
            
            // Shake the panda slightly
            PandaImage.TranslationX = _random.Next(-5, 6);
            PandaImage.TranslationY = _random.Next(-5, 6);
        }
        else
        {
            PandaImage.Source = "meme_panda_stress_3.svg"; // The most stressed one
            StatusLabel.Text = "PANDA IS PANICKING!";
            StressBar.ProgressColor = Colors.Red;
            
            // Shake more violently
            PandaImage.TranslationX = _random.Next(-15, 16);
            PandaImage.TranslationY = _random.Next(-15, 16);
        }
    }

    private void OnPandaClicked(object sender, EventArgs e)
    {
        if (_isGameOver)
        {
            StartGame();
            return;
        }

        // Calming the panda reduces stress
        _stressLevel = Math.Max(0, _stressLevel - 0.15);
        StressBar.Progress = _stressLevel;
        
        // Return to normal position
        PandaImage.TranslationX = 0;
        PandaImage.TranslationY = 0;
        
        UpdatePandaState();

        // Juice: quick pulse
        PandaImage.ScaleTo(1.1, 50).ContinueWith(t => PandaImage.ScaleTo(1.0, 50));
    }

    private async void GameOver()
    {
        _isGameOver = true;
        _gameTimer.Stop();
        StatusLabel.Text = "PANDA EXPLODED!";
        
        int finalScore = _secondsSurvived / 10;
        
        bool isLoggedIn = await _apiService.IsLoggedInAsync();
        if (isLoggedIn && finalScore > 0)
        {
            bool success = await _apiService.SubmitScoreAsync(finalScore, "Panda");
            if (success)
            {
                await DisplayAlert("Panic!", $"The Panda couldn't take the stress anymore! You survived for {finalScore} seconds. Score saved to Leaderboard!", "Try Again");
            }
            else
            {
                await DisplayAlert("Panic!", $"The Panda couldn't take the stress anymore! You survived for {finalScore} seconds. (Failed to save score to backend).", "Try Again");
            }
        }
        else
        {
            await DisplayAlert("Panic!", $"The Panda couldn't take the stress anymore! You survived for {finalScore} seconds.", "Try Again");
        }
        
        StartGame();
    }
}