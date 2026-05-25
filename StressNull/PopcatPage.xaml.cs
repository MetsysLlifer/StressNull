using Plugin.Maui.Audio;
using StressNull.Services;

namespace StressNull;

public partial class PopcatPage : ContentPage
{
    private int _score = 0;
    private readonly IAudioManager _audioManager;
    private readonly ApiService _apiService;
    private IAudioPlayer? _popPlayer;

    public PopcatPage(IAudioManager audioManager, ApiService apiService)
    {
        InitializeComponent();
        _audioManager = audioManager;
        _apiService = apiService;
        LoadAudio();
    }

    private async void LoadAudio()
    {
        _popPlayer = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("pop-cat-original-meme.mp3"));
    }

    private async void OnPopcatPressed(object sender, EventArgs e)
    {
        PopcatImage.Source = "popcat_open.svg";
        _score++;
        ScoreLabel.Text = _score.ToString();

        if (_popPlayer != null)
        {
            double master = Preferences.Default.Get("MasterVolume", 1.0);
            double sfx = Preferences.Default.Get("SfxVolume", 0.5);
            _popPlayer.Volume = Math.Clamp(master * sfx, 0, 2);
            _popPlayer.Play();
        }

        await PopcatImage.ScaleTo(0.95, 50, Easing.CubicOut);
    }

    private async void OnPopcatReleased(object sender, EventArgs e)
    {
        PopcatImage.Source = "popcat_closed.svg";
        await PopcatImage.ScaleTo(1.0, 50, Easing.CubicIn);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (_score > 0)
        {
            _ = _apiService.SubmitScoreAsync(_score, "Popcat");
        }
    }
}