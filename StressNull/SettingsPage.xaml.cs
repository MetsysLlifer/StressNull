namespace StressNull;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
        LoadSettings();
    }

    private void LoadSettings()
    {
        MasterVolumeSlider.Value = Preferences.Default.Get("MasterVolume", 1.0);
        MusicVolumeSlider.Value = Preferences.Default.Get("MusicVolume", 0.5);
        SfxVolumeSlider.Value = Preferences.Default.Get("SfxVolume", 0.5);
    }

    private void OnVolumeChanged(object sender, ValueChangedEventArgs e)
    {
        if (sender == MasterVolumeSlider)
        {
            Preferences.Default.Set("MasterVolume", e.NewValue);
        }
        else if (sender == MusicVolumeSlider)
        {
            Preferences.Default.Set("MusicVolume", e.NewValue);
        }
        else if (sender == SfxVolumeSlider)
        {
            Preferences.Default.Set("SfxVolume", e.NewValue);
        }

        // Always update background music if master or music volume changes
        if (sender == MasterVolumeSlider || sender == MusicVolumeSlider)
        {
            if (App.Current is App app)
            {
                app.UpdateMusicVolume();
            }
        }
    }
}