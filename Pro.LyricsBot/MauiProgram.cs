using Microsoft.Extensions.Logging;
using Pro.LyricsBot.Pages;
using Pro.LyricsBot.Services;
using Pro.LyricsBot.ViewModels;

namespace Pro.LyricsBot
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IModelServiceProvider, ModelServiceProvider>();
            builder.Services.AddSingleton<IAudioSourceProvider, AudioSourceProvider>();
            builder.Services.AddSingleton<ISettingsVM, SettingsVM>();
            builder.Services.AddSingleton<ISettings, SettingsVM>();

            builder.Services.AddScoped(serviceProvider => new Settings()
            {
                BindingContext = serviceProvider.GetRequiredService<ISettingsVM>()
            });


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
