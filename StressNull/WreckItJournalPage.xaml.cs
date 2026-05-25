using StressNull.Services;

namespace StressNull;

public partial class WreckItJournalPage : ContentPage
{
    private const string LastClearedDateKey = "WreckItJournal_LastClearedDate";
    private const string JournalTextKey = "WreckItJournal_Text";
    private readonly ApiService _apiService;

    public WreckItJournalPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CheckAndClearJournal();
    }

    private void CheckAndClearJournal()
    {
        string lastClearedStr = Preferences.Default.Get(LastClearedDateKey, string.Empty);
        DateTime lastCleared;
        
        bool hasDate = DateTime.TryParse(lastClearedStr, out lastCleared);

        // If it's a new day (or never cleared before), wipe it
        if (!hasDate || lastCleared.Date < DateTime.Now.Date)
        {
            JournalEditor.Text = string.Empty;
            Preferences.Default.Set(JournalTextKey, string.Empty);
            Preferences.Default.Set(LastClearedDateKey, DateTime.Now.ToString("O"));
        }
        else
        {
            // Load saved text for today
            JournalEditor.Text = Preferences.Default.Get(JournalTextKey, string.Empty);
        }
    }

    private void OnJournalTextChanged(object sender, TextChangedEventArgs e)
    {
        // Save as they type
        Preferences.Default.Set(JournalTextKey, JournalEditor.Text);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        int score = (JournalEditor.Text?.Length ?? 0) / 5;
        if (score > 0)
        {
            _ = _apiService.SubmitScoreAsync(score, "Journal");
        }
    }
}