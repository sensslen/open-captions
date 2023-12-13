namespace Pro.LyricsBot.Services
{
    public interface IAudioToTextService : IDisposable
    {
        IObservable<string> WhenRecognizedTextChanged { get; }
        IObservable<string> WhenRecognitionEnded { get; }
    }
}
