using System.ComponentModel;

namespace Pro.LyricsBot.Services
{
    public interface IWritableSettings : ISettings, INotifyPropertyChanged
    {
        new string ProPresenterHost { get; set; }
        new int ProPresenterPort { get; set; }
        new int LineLength { get; set; }
        new int LineCount { get; set; }
        new string MessageId { get; set; }
        new string TokenName { get; set; }
    }
}
