namespace Pro.LyricsBot.Services
{
    public interface IAudioToTextService
    {
        IObservable<string> WhenRecognizedTextChanged { get; }
        IObservable<string> WhenRecognitionEnded { get; }
    }
}
