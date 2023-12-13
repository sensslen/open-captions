namespace Pro.LyricsBot.Services
{
    public interface ISettings
    {
        IAudioDeviceDescription? SelectedAudioSource { get; }
        string ProPresenterHost { get; }
        int ProPresenterPort { get; }
        int LineLength { get; }
        int LineCount { get; }
        string MessageId { get; }
        string TokenName { get; }
    }
}
