using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using StressNull.Services;

namespace StressNull;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .AddAudio()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Configure HttpClient for the backend API
        // NOTE: Replace 'your-app-name' with the actual subdomain provided by Back4App once deployed.
        string baseAddress = "https://your-app-name.b4a.run/";
        
        builder.Services.AddHttpClient<ApiService>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
        });

        // Register Services
        builder.Services.AddSingleton(AudioManager.Current);
        
        // Register Pages
        builder.Services.AddTransient<PopcatPage>();
        builder.Services.AddTransient<PlayPage>();
        builder.Services.AddTransient<MainMenuPage>();
        builder.Services.AddTransient<SettingsPage>();
        builder.Services.AddTransient<AboutPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<LeaderboardPage>();

        return builder.Build();
    }
}