using Plugin.Maui.Audio;

namespace StressNull;

public partial class App : Application
{
    private IAudioPlayer? _bgMusic;

    public App(MainMenuPage mainMenuPage)
    {
        InitializeComponent();

        // Force light theme regardless of system settings
        UserAppTheme = AppTheme.Light;

        MainPage = new NavigationPage(mainMenuPage);
        
        LoadBackgroundMusic();
    }

    private async void LoadBackgroundMusic()
    {
        try 
        {
            _bgMusic = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("StressNull MBG.mp3"));
            _bgMusic.Loop = true;
            UpdateMusicVolume();
            _bgMusic.Play();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading background music: {ex.Message}");
        }
    }

    public void UpdateMusicVolume()
    {
        if (_bgMusic != null)
        {
            double master = Preferences.Default.Get("MasterVolume", 1.0);
            double music = Preferences.Default.Get("MusicVolume", 0.5);
            
            // Final volume can be up to 2.0 (if master=2, music=1)
            _bgMusic.Volume = Math.Clamp(master * music, 0, 2);
        }
    }
}