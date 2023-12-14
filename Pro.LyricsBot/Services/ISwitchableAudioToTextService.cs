namespace Pro.LyricsBot.Services
{
    public interface ISwitchableAudioToTextService : IAudioToTextService
    {
        void Switch(IAudioToTextService audioToTextService);
    }
}
