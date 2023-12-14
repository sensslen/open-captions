using System.Text.Json.Serialization;
using Pro.LyricsBot.Pages;
using RestSharp;

namespace Pro.LyricsBot.Services
{

    public class SendToProPresenterService : DisposableBase, ISendToProPresenterService
    {
        private RestClient _client;
        private readonly Task<Message?> _getMessageTask;
        private readonly CancellationTokenSource _cancelAsyncTasks = new CancellationTokenSource();
        private readonly ISettings _settings;

        public SendToProPresenterService(ISettings settings)
        {
            _client = new RestClient();
            _settings = settings;
            _getMessageTask = GetMessageAsync();
        }

        private async Task<Message?> GetMessageAsync()
        {
            var request = new RestRequest(GetMessagePath());
            while (true)
            {
                var response = await _client.ExecuteGetAsync<Message>(request, _cancelAsyncTasks.Token);
                if (response.IsSuccessful)
                {
                    return response.Data;
                }
            }
        }

        public async Task<bool> SendAsync(string text)
        {
            var message = await _getMessageTask;
            if (message is not null)
            {
                if (!await SendTextAsync(text, message))
                {
                    return false;
                }
                return await ShowMessageAsync();
            }
            return false;
        }

        protected override void OnDispose()
        {
            _cancelAsyncTasks.Cancel();
        }
        private string GetMessagePath() => $"{_settings.ProPresenterHost}:{_settings.ProPresenterPort}/v1/message/{_settings.MessageId}";

        private async Task<bool> SendTextAsync(string text, Message message)
        {
            var sendMessage = message with { Value = text };
            var request = new RestRequest(GetMessagePath());
            request.AddObject(sendMessage);
            var result = await _client.ExecutePutAsync(request);
            return result.IsSuccessful;
        }

        private async Task<bool> ShowMessageAsync()
        {
            var sendMessage = Array.Empty<Token>();
            var request = new RestRequest($"{GetMessagePath()}/trigger");
            request.AddObject(sendMessage);
            var result = await _client.ExecutePostAsync(request);
            return result.IsSuccessful;
        }

        private sealed record Id([property: JsonPropertyName("index")] int Index,
           [property: JsonPropertyName("name")] string? Name,
           [property: JsonPropertyName("uuid")] string? Uuid);

        private sealed record Token();

        private sealed record Message([property: JsonPropertyName("id")] Id Id,
            [property: JsonPropertyName("message")] string? Value,
            [property: JsonPropertyName("tokens")] Token[] Tokens,
            [property: JsonPropertyName("theme")] Id Theme,
            [property: JsonPropertyName("visible_on_Network")] bool VisibleOnNetwork);
    }
}
