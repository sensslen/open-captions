using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pro.LyricsBot.Services;
using System.Collections.ObjectModel;

namespace Pro.LyricsBot.ViewModels
{
    public partial class SettingsVM : ObservableObject, ISettingsVM, ISettings
    {
        private IAudioToTextService? _audioToTextService;

        public ObservableCollection<IAudioDeviceDescription> Devices { get; } = new ObservableCollection<IAudioDeviceDescription>();
        public ObservableCollection<IVoskModelDescriptor> Models { get; } = new ObservableCollection<IVoskModelDescriptor>();

        [ObservableProperty]
        private IAudioDeviceDescription? _selectedAudioSource;

        partial void OnSelectedAudioSourceChanged(IAudioDeviceDescription? value) => Preferences.Default.Set(nameof(SelectedAudioSource), value?.Id ?? string.Empty);

        [ObservableProperty]
        private IVoskModelDescriptor? _selectedTranscriptionModel;

        partial void OnSelectedTranscriptionModelChanged(IVoskModelDescriptor? value) => Preferences.Default.Set(nameof(SelectedTranscriptionModel), value?.Id ?? string.Empty);


        [ObservableProperty]
        private string _proPresenterHost = Preferences.Default.Get(nameof(ProPresenterHost), string.Empty);

        partial void OnProPresenterHostChanged(string value) => Preferences.Default.Set(nameof(ProPresenterHost), value ?? string.Empty);


        [ObservableProperty]
        private int _proPresenterPort = Preferences.Default.Get(nameof(ProPresenterPort), 80);

        partial void OnProPresenterPortChanged(int value) => Preferences.Default.Set(nameof(ProPresenterPort), value);


        [ObservableProperty]
        private int _lineLength = Preferences.Default.Get(nameof(LineLength), 100);

        partial void OnLineLengthChanged(int value) => Preferences.Default.Set(nameof(LineLength), value);


        [ObservableProperty]
        private int _lineCount = Preferences.Default.Get(nameof(LineCount), 2);

        partial void OnLineCountChanged(int value) => Preferences.Default.Set(nameof(LineCount), value);


        [ObservableProperty]
        private string _messageId = Preferences.Default.Get(nameof(MessageId), string.Empty);

        partial void OnMessageIdChanged(string value) => Preferences.Default.Set(nameof(MessageId), value);


        [ObservableProperty]
        private string _tokenName = Preferences.Default.Get(nameof(TokenName), string.Empty);

        partial void OnTokenNameChanged(string value) => Preferences.Default.Set(nameof(TokenName), value);

        [ObservableProperty]
        private string _startStopLabel = "Start";
        private readonly IAudioSourceProvider _audioSourceProvider;
        private readonly IModelServiceProvider _modelServiceProvider;

        [RelayCommand]
        private void StartStop()
        {
            if (StartStopLabel == "Start" && SelectedTranscriptionModel is not null && SelectedAudioSource is not null)
            {
                _audioToTextService = new AudioToTextService(_modelServiceProvider.Get(SelectedTranscriptionModel), _audioSourceProvider.Open(SelectedAudioSource));

                StartStopLabel = "Stop";
            }
            else
            {
                _audioToTextService?.Dispose();
                _audioToTextService = null;

                StartStopLabel = "Start";
            }
        }

        public SettingsVM(IAudioSourceProvider audioSourceProvider, IModelServiceProvider modelServiceProvider)
        {
            var previousDeviceSelection = Preferences.Default.Get(nameof(SelectedAudioSource), string.Empty);
            foreach (var device in audioSourceProvider.GetAvailable())
            {
                Devices.Add(device);
                if (device.Id == previousDeviceSelection)
                {
                    SelectedAudioSource = device;
                }
            }
            var previousModelSelection = Preferences.Default.Get(nameof(SelectedTranscriptionModel), string.Empty);
            foreach (var model in modelServiceProvider.Available())
            {
                Models.Add(model);
                if (model.Id == previousModelSelection)
                {
                    SelectedTranscriptionModel = model;
                }
            }
            _audioSourceProvider = audioSourceProvider;
            _modelServiceProvider = modelServiceProvider;
        }
    }
}
