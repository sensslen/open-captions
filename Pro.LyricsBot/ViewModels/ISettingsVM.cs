using System.Collections.ObjectModel;
using System.ComponentModel;
using Pro.LyricsBot.Services;

namespace Pro.LyricsBot.ViewModels
{
    public interface ISettingsVM : INotifyPropertyChanged
    {
        public ObservableCollection<IAudioDeviceDescription> Devices { get; }

        public IAudioDeviceDescription? SelectedAudioSource { get; set; }
        string? ProPresenterHost { get; set; }
        int ProPresenterPort { get; set; }
        int LineLength { get; set; }
        int LineCount { get; set; }
        string MessageId { get; set; }
        string TokenName { get; set; }
    }
}
