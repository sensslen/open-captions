// Copyright (c) Renewed Vision, LLC. All rights reserved.

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
            _getMessageTask = GetMessageAsync();
            _settings = settings;
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
            return await ShowMessageAsync(text);
        }

        protected override void OnDispose()
        {
            _cancelAsyncTasks.Cancel();
            _textFormattingSubscription.Dispose();
        }

        private async Task<bool> ShowMessageAsync(string text)
        {
            var tokenText = new TokenText(text);
            var token = new Token(_settings.TokenName, tokenText);
            var sendMessage = Array.Empty<Token>().Append(token);

            var request = new RestRequest($"v1/message/{_settings.MessageId}/trigger");
            request.AddObject(sendMessage);
            var result = await _client.ExecutePostAsync(request, _cancelAsyncTasks.Token);
            return result.IsSuccessful;
        }

        private sealed record TokenText(
            [property: JsonPropertyName("text")] string? Text
            );

        private sealed record Token(
            [property: JsonPropertyName("name")] string? Name,
            [property: JsonPropertyName("text")] TokenText TokenText
            );

    }
}
