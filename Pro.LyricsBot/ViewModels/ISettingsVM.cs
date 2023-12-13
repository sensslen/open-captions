using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pro.LyricsBot.Services;

namespace Pro.LyricsBot.ViewModels
{
    public interface ISettingsVM : INotifyPropertyChanged
    {
        public ObservableCollection<IAudioDeviceDescription> Devices { get; }
        public ObservableCollection<IVoskModelDescriptor> Models { get; }

        public IAudioDeviceDescription? SelectedAudioSource { get; set; }
        public IVoskModelDescriptor? SelectedTranscriptionModel { get; set; }
        string? ProPresenterHost { get; set; }
        int ProPresenterPort { get; set; }
        int LineLength { get; set; }
        int LineCount { get; set; }
        string MessageId { get; set; }
        string TokenName { get; set; }
        string StartStopLabel { get; }
        string TranscribedText { get; }
        IRelayCommand StartStopCommand { get; }
    }
}
