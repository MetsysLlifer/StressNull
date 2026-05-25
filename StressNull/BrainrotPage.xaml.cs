using System.Collections.ObjectModel;
using Plugin.Maui.Audio;
using StressNull.Services;

namespace StressNull;

public class BrainrotItem
{
    public string ImageSource { get; set; } = string.Empty;
    public string SoundSource { get; set; } = string.Empty;
}

public partial class BrainrotPage : ContentPage
{
    private int _memesConsumed = 0;
    private double _dopamine = 0.1;
    private readonly IAudioManager _audioManager;
    private readonly ApiService _apiService;
    private readonly Dictionary<string, IAudioPlayer> _audioPlayers = new();
    private readonly Random _random = new();

    private readonly List<BrainrotItem> _availableItems = new()
    {
        new() { ImageSource = "random_cat_1.svg", SoundSource = "vine-boom.mp3" }, 
        new() { ImageSource = "random_cat_2.svg", SoundSource = "huh-cat-meme.mp3" },
        new() { ImageSource = "random_cat_3.svg", SoundSource = "cat-rap.mp3" },
        new() { ImageSource = "random_cat_4.svg", SoundSource = "spongebob-fail.mp3" },
        new() { ImageSource = "random_cat_5.svg", SoundSource = "confused-cross-eyed-kitten-meme.mp3" },
        new() { ImageSource = "random_dog_1.svg", SoundSource = "what-the-dog-doin.mp3" },
        new() { ImageSource = "random_dog_2.svg", SoundSource = "bruh.mp3" },
        new() { ImageSource = "baby_1.svg", SoundSource = "spongebob-fail.mp3" },
        new() { ImageSource = "baby_2.svg", SoundSource = "vine-boom.mp3" },
        new() { ImageSource = "happy_man.svg", SoundSource = "bruh.mp3" },
        new() { ImageSource = "meme_monkey.svg", SoundSource = "metal-pipe-clang.mp3" },
        new() { ImageSource = "meme_jackie.svg", SoundSource = "ching-chong-chang-song.mp3" }
    };

    public BrainrotPage(ApiService apiService)
    {
        InitializeComponent();
        _audioManager = AudioManager.Current;
        _apiService = apiService;
        
        // Initial setup: add a few cards to the stack
        for (int i = 0; i < 3; i++)
        {
            AddCardToStack();
        }
    }

    private async Task<IAudioPlayer> GetAudioPlayer(string soundFile)
    {
        if (_audioPlayers.TryGetValue(soundFile, out var player))
            return player;

        var newPlayer = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync(soundFile));
        _audioPlayers[soundFile] = newPlayer;
        return newPlayer;
    }

    private void AddCardToStack()
    {
        var item = _availableItems[_random.Next(_availableItems.Count)];
        
        var cardImage = new Image
        {
            Source = item.ImageSource,
            Aspect = Aspect.AspectFit,
            BindingContext = item,
            Scale = 0.95, // Slightly smaller for stack effect
            Shadow = new Shadow
            {
                Brush = Colors.Black,
                Offset = new Point(0, 5),
                Radius = 15,
                Opacity = 0.1f
            }
        };

        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;
        cardImage.GestureRecognizers.Add(panGesture);

        // Add to the bottom of the children so it's behind others
        // But for swiping, the last added is on top.
        CardContainer.Children.Insert(0, cardImage);
        
        // Adjust scales for stack effect
        UpdateStackVisuals();
    }

    private void UpdateStackVisuals()
    {
        for (int i = 0; i < CardContainer.Children.Count; i++)
        {
            var view = (View)CardContainer.Children[i];
            // The last item (highest index) is on top
            int indexFromTop = CardContainer.Children.Count - 1 - i;
            
            view.Scale = Math.Max(0.8, 1.0 - (indexFromTop * 0.05));
            view.Opacity = Math.Max(0.5, 1.0 - (indexFromTop * 0.3));
            view.TranslationY = indexFromTop * 10;
            
            // Add a slight random-looking rotation to the stack
            view.Rotation = (indexFromTop * 3) * (i % 2 == 0 ? 1 : -1);
            
            view.InputTransparent = indexFromTop > 0; // Only top card is interactive
        }
    }

    private double _startX, _startY;

    private async void OnPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        if (sender is not View view) return;

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                _startX = view.TranslationX;
                _startY = view.TranslationY;
                break;

            case GestureStatus.Running:
                view.TranslationX = _startX + e.TotalX;
                view.TranslationY = _startY + e.TotalY;
                // Add a little rotation for "throwing" feel
                view.Rotation = view.TranslationX * 0.1;
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                // Check if it's "thrown" far enough (lowered threshold to 60 for easier swiping)
                if (Math.Abs(view.TranslationX) > 60 || Math.Abs(view.TranslationY) > 60)
                {
                    await ThrowCardAway(view);
                }
                else
                {
                    // Snap back
                    await Task.WhenAll(
                        view.TranslateTo(0, 0, 200, Easing.SpringOut),
                        view.RotateTo(0, 200, Easing.SpringOut)
                    );
                }
                break;
        }
    }

    private async Task ThrowCardAway(View view)
    {
        var item = (BrainrotItem)view.BindingContext;
        
        // Determine direction to fly off
        double targetX = view.TranslationX * 5;
        double targetY = view.TranslationY * 5;

        _memesConsumed++;
        ScoreLabel.Text = $"MEMES CONSUMED: {_memesConsumed}";

        // Dopamine rush
        _dopamine = Math.Min(1.0, _dopamine + 0.05);
        DopamineBar.Progress = _dopamine;
        UpdateDopamineColor();

        // Play sound
        PlaySound(item.SoundSource);

        // Animate off screen
        await Task.WhenAll(
            view.TranslateTo(targetX, targetY, 400, Easing.CubicIn),
            view.RotateTo(view.Rotation * 2, 400, Easing.CubicIn),
            view.FadeTo(0, 400)
        );

        // Remove and add new one
        CardContainer.Children.Remove(view);
        AddCardToStack();
    }

    private async void PlaySound(string soundFile)
    {
        try
        {
            var player = await GetAudioPlayer(soundFile);
            double master = Preferences.Default.Get("MasterVolume", 1.0);
            double sfx = Preferences.Default.Get("SfxVolume", 0.5);
            player.Volume = Math.Clamp(master * sfx, 0, 2);
            player.Play();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error playing sound: {ex.Message}");
        }
    }

    private void UpdateDopamineColor()
    {
        if (_dopamine > 0.8)
            DopamineBar.ProgressColor = Colors.Red;
        else if (_dopamine > 0.5)
            DopamineBar.ProgressColor = Colors.Orange;
        else
            DopamineBar.ProgressColor = Colors.Lime;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (_memesConsumed > 0)
        {
            _ = _apiService.SubmitScoreAsync(_memesConsumed, "Brainrot");
        }
    }
}