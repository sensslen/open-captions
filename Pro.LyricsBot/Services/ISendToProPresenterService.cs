namespace Pro.LyricsBot.Services
{
    public interface ISendToProPresenterService : IDisposable
    {
        Task<bool> SendAsync(string text);
    }
}
