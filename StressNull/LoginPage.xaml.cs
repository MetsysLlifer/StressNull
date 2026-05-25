using StressNull.Services;

namespace StressNull;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService;

    public LoginPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;

        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ErrorLabel.Text = "Please enter username and password.";
            ErrorLabel.IsVisible = true;
            return;
        }

        var errorMessage = await _apiService.LoginAsync(username, password);

        if (errorMessage == null)
        {
            // Login successful, go back to main menu
            await Navigation.PopAsync();
        }
        else
        {
            ErrorLabel.Text = errorMessage;
            ErrorLabel.IsVisible = true;
        }
    }

    private async void OnRegisterTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage(_apiService));
    }
}