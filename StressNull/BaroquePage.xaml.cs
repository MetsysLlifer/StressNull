using Plugin.Maui.Audio;
using StressNull.Services;

namespace StressNull;

public partial class BaroquePage : ContentPage
{
    private readonly string[] _baroqueResponses = 
    {
        "Verily, I hear thee.",
        "Tis so, indeed.",
        "Aye.",
        "Thy words are acknowledged.",
        "As thou sayest.",
        "I perceive thy plight.",
        "Okayeth.",
        "Yes, my liege."
    };

    private readonly IAudioManager _audioManager;
    private readonly ApiService _apiService;
    private IAudioPlayer _keystrokePlayer;
    private IAudioPlayer _celloPlayer;
    private readonly Random _random = new();

    public BaroquePage(IAudioManager audioManager, ApiService apiService)
    {
        InitializeComponent();
        _audioManager = audioManager;
        _apiService = apiService;
        LoadSounds();
    }

    private async void LoadSounds()
    {
        try 
        {
            var keystrokeStream = await FileSystem.OpenAppPackageFileAsync("harpsichord_pluck.mp3");
            _keystrokePlayer = _audioManager.CreatePlayer(keystrokeStream);
            
            var celloStream = await FileSystem.OpenAppPackageFileAsync("cello_swell.mp3");
            _celloPlayer = _audioManager.CreatePlayer(celloStream);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading sounds: {ex.Message}");
        }
    }

    private void OnVentTextChanged(object sender, TextChangedEventArgs e)
    {
        // Play harpsichord pluck on keystroke if loaded and not currently playing
        if (_keystrokePlayer != null)
        {
            if (_keystrokePlayer.IsPlaying)
                _keystrokePlayer.Stop();
            
            _keystrokePlayer.Play();
        }
    }

    private void OnVentCompleted(object sender, EventArgs e)
    {
        Unburden();
    }

    private void OnUnburdenClicked(object sender, EventArgs e)
    {
        Unburden();
    }

    private async void Unburden()
    {
        if (string.IsNullOrWhiteSpace(VentEditor.Text))
        {
            ResponseLabel.Text = "Speak, lest the silence consume thee entirely.";
            return;
        }

        int score = VentEditor.Text.Length;

        // Play dramatic cello swell
        if (_celloPlayer != null)
        {
            if (_celloPlayer.IsPlaying)
                _celloPlayer.Stop();
            _celloPlayer.Play();
        }

        // Dramatic fade out and in
        await ResponseLabel.FadeTo(0, 500);

        int index = _random.Next(_baroqueResponses.Length);
        ResponseLabel.Text = _baroqueResponses[index];
        VentEditor.Text = string.Empty; 

        await ResponseLabel.FadeTo(1, 500);

        bool isLoggedIn = await _apiService.IsLoggedInAsync();
        if (isLoggedIn && score > 0)
        {
            await _apiService.SubmitScoreAsync(score, "Baroque");
        }
    }
}