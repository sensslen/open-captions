namespace Pro.LyricsBot.Services
{
    public interface ISettings
    {
        string ProPresenterHost { get; }
        int ProPresenterPort { get; }
        int LineLength { get; }
        int LineCount { get; }
        string MessageId { get; }
        string TokenName { get; }
    }
}
