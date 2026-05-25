using StressNull.Services;

namespace StressNull;

public partial class UnsolicitedAdvicePage : ContentPage
{
    private readonly string[] _advices = {
        "Remember, if you drop your keys in lava, let 'em go, because man, they're gone.",
        "You can't have a mid-life crisis if you never grow up.",
        "If at first you don't succeed, skydiving is not for you.",
        "Never put off till tomorrow what you can avoid altogether.",
        "If you sleep until noon, you only have to pay for two meals.",
        "Just because you are trash doesn't mean you can't do great things. It is called a garbage CAN, not a garbage CANNOT.",
        "A balanced diet means a cupcake in each hand.",
        "If you think nobody cares if you're alive, try missing a couple of car payments."
    };
    
    private readonly Random _random = new();
    private readonly ApiService _apiService;
    private int _adviceCount = 0;

    public UnsolicitedAdvicePage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void OnGiveAdviceClicked(object sender, EventArgs e)
    {
        _adviceCount++;

        // Dramatic fade
        await AdviceLabel.FadeTo(0, 200);
        AdviceLabel.Text = _advices[_random.Next(_advices.Length)];
        await AdviceLabel.ScaleTo(1.2, 100);
        await AdviceLabel.FadeTo(1, 200);
        await AdviceLabel.ScaleTo(1.0, 100);
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (_adviceCount > 0)
        {
            _ = _apiService.SubmitScoreAsync(_adviceCount, "Advice");
        }
    }
}