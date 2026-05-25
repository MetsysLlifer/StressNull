using Plugin.Maui.Audio;
using StressNull.Services;

namespace StressNull;

public partial class PlayPage : ContentPage
{
    private readonly IAudioManager _audioManager;
    private readonly ApiService _apiService;

    public PlayPage(IAudioManager audioManager, ApiService apiService)
    {
        InitializeComponent();
        _audioManager = audioManager;
        _apiService = apiService;
    }

    private async void OnPopcatTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PopcatPage(_audioManager, _apiService));
    }

    private async void OnBrainrotTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BrainrotPage(_apiService));
    }

    private async void OnPandaTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new PandaPage(_apiService));
    }

    private async void OnBureaucracyTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BureaucracyPage(_audioManager, _apiService));
    }

    private async void OnBaroqueTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BaroquePage(_audioManager, _apiService));
    }

    private async void OnAdviceTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UnsolicitedAdvicePage(_apiService));
    }

    private async void OnJournalTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WreckItJournalPage(_apiService));
    }

    private async void OnScreamTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ScreamLogPage(_apiService));
    }
}