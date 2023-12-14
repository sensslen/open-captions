using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Pro.LyricsBot.ViewModels
{
    public interface ISettingsVM : INotifyPropertyChanged
    {
        public IEnumerable<IModelSettingsVM> ModelSettingsProviders { get; }
        public IModelSettingsVM SelectedModelSettingsProvider { get; }
        string ProPresenterHost { get; set; }
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
