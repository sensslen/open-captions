using CommunityToolkit.Mvvm.ComponentModel;

namespace Pro.LyricsBot.Services
{
    public partial class Settings : ObservableObject, IWritableSettings
    {
        [ObservableProperty]
        private string _proPresenterHost = GetPreference(nameof(ProPresenterHost), string.Empty);
        partial void OnProPresenterHostChanged(string value) => SetPreference(nameof(ProPresenterHost), value);

        [ObservableProperty]
        private int _proPresenterPort = GetPreference(nameof(ProPresenterPort), 80);
        partial void OnProPresenterPortChanged(int value) => SetPreference(nameof(ProPresenterPort), value);

        [ObservableProperty]
        private int _lineLength = GetPreference(nameof(LineLength), 100);
        partial void OnLineLengthChanged(int value) => SetPreference(nameof(LineLength), value);

        [ObservableProperty]
        private int _lineCount = GetPreference(nameof(LineCount), 4);
        partial void OnLineCountChanged(int value) => SetPreference(nameof(LineCount), value);

        [ObservableProperty]
        private string _messageId = GetPreference(nameof(MessageId), string.Empty);
        partial void OnMessageIdChanged(string value) => SetPreference(nameof(MessageId), value);

        [ObservableProperty]
        private string _tokenName = GetPreference(nameof(TokenName), string.Empty);
        partial void OnTokenNameChanged(string value) => SetPreference(nameof(TokenName), value);

        private static T GetPreference<T>(string key, T defaultValue) => Preferences.Default.Get($"{nameof(Settings)}.{key}", defaultValue);
        private void SetPreference<T>(string key, T value) => Preferences.Default.Set($"{nameof(Settings)}.{key}", value);
    }
}
