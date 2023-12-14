// Copyright (c) Renewed Vision, LLC. All rights reserved.

using System.Text.Json.Serialization;
using RestSharp;

namespace Pro.LyricsBot.Services
{

    public class SendToProPresenterService : DisposableBase, ISendToProPresenterService
    {
        private RestClient _client;
        private readonly CancellationTokenSource _cancelAsyncTasks = new CancellationTokenSource();
        private readonly ISettings _settings;

        public SendToProPresenterService(ISettings settings)
        {
            _client = new RestClient();
            _settings = settings;
        }

        public async Task<bool> SendAsync(string text)
        {
            return await ShowMessageAsync(text);
        }

        protected override void OnDispose()
        {
            _cancelAsyncTasks.Cancel();
        }

        private async Task<bool> ShowMessageAsync(string text)
        {
            var tokenText = new TokenText(text);
            var token = new Token(_settings.TokenName, tokenText);

            var request = new RestRequest($"http://{_settings.ProPresenterHost}:{_settings.ProPresenterPort}/v1/message/{_settings.MessageId}/trigger");
            request.AddBody(new[] { token });
            try
            {
                var result = await _client.ExecutePostAsync(request, _cancelAsyncTasks.Token);
                return result.IsSuccessful;
            }
            catch
            {
                return false;
            }
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
