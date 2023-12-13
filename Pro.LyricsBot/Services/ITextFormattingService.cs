namespace Pro.LyricsBot.Services
{
    public interface ITextFormattingService
    {
        IObservable<string> WhenTextChanged { get; }
    }
}
