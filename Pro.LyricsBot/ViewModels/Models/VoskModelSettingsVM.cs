// Copyright (c) Renewed Vision, LLC. All rights reserved.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pro.LyricsBot.Services;
using Pro.LyricsBot.Services.VoskTranscriber;

namespace Pro.LyricsBot.ViewModels.Models
{
    public partial class VoskModelSettingsVM : ObservableObject, IVoskModelSettingsVM
    {
        private readonly IAudioSourceProvider _audioSourceProvider;
        private readonly IVoskModelProvider _voskModelProvider;
        private readonly IVoskAudioToTextFactory _voskAudioToTextFactory;

        public VoskModelSettingsVM(IAudioSourceProvider audioSourceProvider, IVoskModelProvider voskModelProvider, IVoskAudioToTextFactory voskAudioToTextFactory)
        {
            _audioSourceProvider = audioSourceProvider;
            _voskModelProvider = voskModelProvider;
            _voskAudioToTextFactory = voskAudioToTextFactory;

            var previousAudioDeviceSelection = GetPreference(nameof(SelectedAudioSource), string.Empty);
            AvailableAudioSources = _audioSourceProvider.GetAvailable();
            foreach (var device in AvailableAudioSources)
            {
                if (device.Id == previousAudioDeviceSelection)
                {
                    SelectedAudioSource = device;
                }
            }
            var previousModelSelection = GetPreference(nameof(SelectedModel), string.Empty);
            AvailableModels = _voskModelProvider.GetAvailable();
            foreach (var model in AvailableModels)
            {
                if (model.Id == previousModelSelection)
                {
                    SelectedModel = model;
                }
            }
        }

        public IList<ISelectionDescriptor> AvailableModels { get; }

        [ObservableProperty]
        private ISelectionDescriptor? _selectedModel;
        partial void OnSelectedModelChanged(ISelectionDescriptor? value)
        {
            SetPreference(nameof(SelectedModel), value?.Id);
            _voskAudioToTextFactory.Switch(value, SelectedAudioSource);
        }

        public IList<ISelectionDescriptor> AvailableAudioSources { get; }

        [ObservableProperty]
        private ISelectionDescriptor? _selectedAudioSource;
        partial void OnSelectedAudioSourceChanged(ISelectionDescriptor? value)
        {
            SetPreference(nameof(SelectedAudioSource), value?.Id);
            _voskAudioToTextFactory.Switch(SelectedModel, value);
        }

        public string Name => "Vosk Model (transcribed locally)";

        private T GetPreference<T>(string key, T defaultValue) => Preferences.Default.Get($"{nameof(VoskModelSettingsVM)}.{key}", defaultValue);
        private void SetPreference<T>(string key, T value) => Preferences.Default.Set($"{nameof(VoskModelSettingsVM)}.{key}", value);
    }
}
