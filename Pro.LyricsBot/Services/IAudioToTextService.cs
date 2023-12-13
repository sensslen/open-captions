namespace Pro.LyricsBot.Services
{
    public record TextRecognitionResult(string text, bool isEnd);

    public interface IAudioToTextService : IDisposable
    {
        IObservable<TextRecognitionResult> WhenTextChanged { get; }
    }
}
