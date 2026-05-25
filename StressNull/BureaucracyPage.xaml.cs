using Plugin.Maui.Audio;
using StressNull.Services;

namespace StressNull;

public partial class BureaucracyPage : ContentPage
{
    private readonly IAudioManager _audioManager;
    private readonly ApiService _apiService;
    private IAudioPlayer _faxPlayer;
    private IAudioPlayer _stampPlayer;

    public BureaucracyPage(IAudioManager audioManager, ApiService apiService)
    {
        InitializeComponent();
        _audioManager = audioManager;
        _apiService = apiService;
        PermitPicker.ItemsSource = new List<string> { "Beige", "Manila", "Depression Gray" };
        LoadSounds();
    }

    private async void LoadSounds()
    {
        try 
        {
            var faxStream = await FileSystem.OpenAppPackageFileAsync("fax_machine.mp3");
            _faxPlayer = _audioManager.CreatePlayer(faxStream);
            
            var stampStream = await FileSystem.OpenAppPackageFileAsync("stamp_sound.mp3");
            _stampPlayer = _audioManager.CreatePlayer(stampStream);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading sounds: {ex.Message}");
        }
    }

    private async void OnSubmitClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(StressEditor.Text))
        {
            StatusLabel.Text = "Error: Form cannot be blank.";
            return;
        }

        SubmitButton.IsEnabled = false;
        StressEditor.IsEnabled = false;
        
        ProcessingIndicator.IsVisible = true;
        ProcessingIndicator.IsRunning = true;
        StatusLabel.Text = "Dialing fax machine...";
        
        // Play fax sound if loaded
        if (_faxPlayer != null && !_faxPlayer.IsPlaying)
            _faxPlayer.Play();

        // Step 1: Fake Faxing Delay
        await Task.Delay(2500); 
        StatusLabel.Text = "Transmitting to the void...";
        await Task.Delay(2000);

        if (_faxPlayer != null && _faxPlayer.IsPlaying)
            _faxPlayer.Stop();

        ProcessingIndicator.IsRunning = false;
        ProcessingIndicator.IsVisible = false;

        // Step 2: Bureaucratic Hurdle
        PermitContainer.IsVisible = true;
        StatusLabel.Text = "Error: Form 27B/6 missing. Please select a Permit Color.";
    }

    private async void OnPermitSelected(object sender, EventArgs e)
    {
        if (PermitPicker.SelectedIndex == -1) return;

        PermitContainer.IsVisible = false;
        ProcessingIndicator.IsVisible = true;
        ProcessingIndicator.IsRunning = true;
        StatusLabel.Text = "Processing...";
        await Task.Delay(1500);

        ProcessingIndicator.IsRunning = false;
        ProcessingIndicator.IsVisible = false;

        // Step 3: The Stamp Animation
        StampImage.IsVisible = true;
        StampImage.Opacity = 1;
        
        // Play stamp sound if loaded
        if (_stampPlayer != null)
            _stampPlayer.Play();

        // Scale down dramatically like a stamp hitting the paper
        await StampImage.ScaleTo(1.0, 150, Easing.CubicIn); 
        
        try 
        {
            // Haptic feedback
            HapticFeedback.Default.Perform(HapticFeedbackType.LongPress);
        }
        catch { } // Ignore if haptics not supported

        StatusLabel.Text = "Stress note successfully filed in the incinerator.";
        
        bool isLoggedIn = await _apiService.IsLoggedInAsync();
        if (isLoggedIn)
        {
            await _apiService.SubmitScoreAsync(100, "Bureaucracy");
        }
    }
}