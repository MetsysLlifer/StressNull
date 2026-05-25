using StressNull.Services;

namespace StressNull;

public partial class RegisterPage : ContentPage
{
    private readonly ApiService _apiService;

    public RegisterPage(ApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        SuccessLabel.IsVisible = false;

        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ErrorLabel.Text = "Please enter username and password.";
            ErrorLabel.IsVisible = true;
            return;
        }

        var errorMessage = await _apiService.RegisterAsync(username, password);

        if (errorMessage == null)
        {
            SuccessLabel.Text = "Registration successful! You can now log in.";
            SuccessLabel.IsVisible = true;
            await Task.Delay(2000);
            await Navigation.PopAsync();
        }
        else
        {
            ErrorLabel.Text = errorMessage;
            ErrorLabel.IsVisible = true;
        }
    }
}