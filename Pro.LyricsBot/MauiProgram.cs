using Microsoft.Extensions.Logging;
using Pro.LyricsBot.Services;
using Pro.LyricsBot.Services.VoskTranscriber;
using Pro.LyricsBot.ViewModels;
using Pro.LyricsBot.ViewModels.Models;

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

            builder.Services.AddSingleton<IVoskModelProvider, VoskModelProvider>();
            builder.Services.AddSingleton<IAudioSourceProvider, AudioSourceProvider>();
            builder.Services.AddSingleton<ISwitchableAudioToTextService, SwitchableAudioToTextService>();
            builder.Services.AddSingleton<IAudioToTextService>(provider => provider.GetService<ISwitchableAudioToTextService>()!);
            builder.Services.AddSingleton<IVoskAudioToTextFactory, VoskAudioToTextFactory>();
            builder.Services.AddSingleton<ISettingsVM, SettingsVM>();
            builder.Services.AddSingleton<IModelSettingsVM, VoskModelSettingsVM>();
            builder.Services.AddSingleton<ITextFormattingService, TextFormattingService>();
            builder.Services.AddSingleton<IWritableSettings, Services.Settings>();
            builder.Services.AddSingleton<ISettings>(provider => provider.GetService<IWritableSettings>()!);

            builder.Services.AddScoped(serviceProvider => new Pages.Settings()
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
