using Plugin.Maui.Audio;
using StressNull.Services;

namespace StressNull;

public partial class MainMenuPage : ContentPage
{
    private bool _isAnimating;
    private readonly Random _random = new();
    private readonly IAudioManager _audioManager;
    private readonly ApiService _apiService;

    public MainMenuPage(IAudioManager audioManager, ApiService apiService)
    {
        InitializeComponent();
        _audioManager = audioManager;
        _apiService = apiService;
    }

    private bool _quoteShown = false;

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _isAnimating = true;
        await StartAnimationLoop();

        bool isLoggedIn = await _apiService.IsLoggedInAsync();
        UpdateAuthButton(isLoggedIn);

        if (!_quoteShown)
        {
            _quoteShown = true;
            ShowMassiveQuote();
        }
    }

    private void UpdateAuthButton(bool isLoggedIn)
    {
        // Button remains consistently white with a shadow
        // AuthButton.Opacity = isLoggedIn ? 1.0 : 0.8;
    }

    private void ShowMassiveQuote()
    {
        string[] quotes = {
            "WHY ARE YOU STILL HERE?",
            "NO ONE KNOWS WHAT THEY'RE DOING.",
            "HAVE YOU TRIED TURNING IT OFF?",
            "IT IS WHAT IT IS.",
            "EMBRACE THE CHAOS."
        };
        QuoteLabel.Text = quotes[_random.Next(quotes.Length)];
        QuoteOverlay.IsVisible = true;
    }

    private void OnQuoteTapped(object sender, TappedEventArgs e)
    {
        QuoteOverlay.IsVisible = false;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _isAnimating = false;
    }

    private async Task StartAnimationLoop()
    {
        while (_isAnimating)
        {
            var scaleTask = MemeShapesIcon.ScaleTo(1.05, 1000, Easing.SinInOut);
            
            if (_random.NextDouble() < 0.3)
            {
                await MemeShapesIcon.RotateTo(_random.Next(-10, 11), 100, Easing.Linear);
                await MemeShapesIcon.RotateTo(0, 100, Easing.Linear);
            }

            await scaleTask;
            await MemeShapesIcon.ScaleTo(1.0, 1000, Easing.SinInOut);
        }
    }

    private async void OnPlayClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PlayPage(_audioManager, _apiService));
    }

    private async void OnLeaderboardClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LeaderboardPage(_apiService));
    }

    private async void OnAuthClicked(object sender, EventArgs e)
    {
        bool isLoggedIn = await _apiService.IsLoggedInAsync();
        if (isLoggedIn)
        {
            bool confirm = await DisplayAlert("Logout", "Are you sure you want to logout?", "Yes", "No");
            if (confirm)
            {
                await _apiService.LogoutAsync();
                UpdateAuthButton(false);
            }
        }
        else
        {
            await Navigation.PushAsync(new LoginPage(_apiService));
        }
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }

    private async void OnAboutClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AboutPage());
    }
}