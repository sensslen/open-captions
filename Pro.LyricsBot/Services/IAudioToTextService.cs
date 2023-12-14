namespace Pro.LyricsBot.Services
{
    public interface IAudioToTextService : IDisposable
    {
        IObservable<TextRecognitionResult> WhenTextChanged { get; }
    }
}
