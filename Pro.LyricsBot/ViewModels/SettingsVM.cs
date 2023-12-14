using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Pro.LyricsBot.Services;

namespace Pro.LyricsBot.ViewModels
{
    public partial class SettingsVM : ObservableObject, ISettingsVM, IDisposable
    {
        public IEnumerable<IModelSettingsVM> ModelSettingsProviders { get; }

        private IDisposable _transcribedTextSubscription;
        [ObservableProperty]
        private IModelSettingsVM? _selectedModelSettingsProvider;

        partial void OnSelectedModelSettingsProviderChanged(IModelSettingsVM? value) => SetPreference(nameof(SelectedModelSettingsProvider), value?.Name);

        public string ProPresenterHost { get => _settings.ProPresenterHost; set => _settings.ProPresenterHost = value; }
        public int ProPresenterPort { get => _settings.ProPresenterPort; set => _settings.ProPresenterPort = value; }
        public int LineLength { get => _settings.LineLength; set => _settings.LineLength = value; }
        public int LineCount { get => _settings.LineCount; set => _settings.LineCount = value; }
        public string MessageId { get => _settings.MessageId; set => _settings.MessageId = value; }
        public string TokenName { get => _settings.TokenName; set => _settings.TokenName = value; }

        [ObservableProperty]
        private string _startStopLabel = "Start";

        [ObservableProperty]
        private string _transcribedText = string.Empty;
        private bool _disposedValue;
        private readonly IWritableSettings _settings;

        public SettingsVM(IEnumerable<IModelSettingsVM> availableModelSettings, ITextFormattingService textFormattingService, IWritableSettings settings)
        {
            ModelSettingsProviders = availableModelSettings;
            _settings = settings;
            _transcribedTextSubscription = textFormattingService.WhenTextChanged.Subscribe(text => TranscribedText = text);

            var previousModelPorviderSelection = GetPreference(nameof(SelectedModelSettingsProvider), string.Empty);
            foreach (var provider in ModelSettingsProviders)
            {
                if (provider.Name == previousModelPorviderSelection)
                {
                    SelectedModelSettingsProvider = provider;
                }
            }

            _settings.PropertyChanged += OnSettingsPropertyChanged;
        }

        private void OnSettingsPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IWritableSettings.ProPresenterHost): OnPropertyChanged(nameof(ProPresenterHost)); break;
                case nameof(IWritableSettings.ProPresenterPort): OnPropertyChanged(nameof(ProPresenterPort)); break;
                case nameof(IWritableSettings.LineLength): OnPropertyChanged(nameof(LineLength)); break;
                case nameof(IWritableSettings.LineCount): OnPropertyChanged(nameof(LineCount)); break;
                case nameof(IWritableSettings.MessageId): OnPropertyChanged(nameof(MessageId)); break;
                case nameof(IWritableSettings.TokenName): OnPropertyChanged(nameof(TokenName)); break;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _transcribedTextSubscription.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private static T GetPreference<T>(string key, T defaultValue) => Preferences.Default.Get($"{nameof(SettingsVM)}.{key}", defaultValue);
        private void SetPreference<T>(string key, T value) => Preferences.Default.Set($"{nameof(SettingsVM)}.{key}", value);
    }
}
