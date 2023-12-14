using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Text.Json.Serialization;
using NAudio.Wave;
using Vosk;

namespace Pro.LyricsBot.Services.VoskTranscriber
{
    internal class VoskAudioToTextService : DisposableBase, IAudioToTextService
    {
        private readonly SubjectBase<TextRecognitionResult> _textChangedSubject = new ReplaySubject<TextRecognitionResult>(1);
        private readonly VoskRecognizer _recognizer;
        private readonly IWaveIn _audioStream;

        public IObservable<TextRecognitionResult> WhenTextChanged => _textChangedSubject.DistinctUntilChanged();

        public VoskAudioToTextService(Model model, IWaveIn audioStream)
        {
            _audioStream = audioStream;

            _recognizer = new VoskRecognizer(model, audioStream.WaveFormat.SampleRate);
            _recognizer.SetMaxAlternatives(0);
            _recognizer.SetWords(true);

            _audioStream.DataAvailable += AudioDataReceived;
        }

        protected override void OnDispose()
        {
            _audioStream.DataAvailable -= AudioDataReceived;
            _recognizer.Dispose();
            _audioStream.Dispose();
        }

        private void AudioDataReceived(object? sender, WaveInEventArgs e)
        {
            if (_recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
            {
                var json = _recognizer.Result();
                var result = JsonSerializer.Deserialize<RecognizerResult>(json);
                if (result is not null)
                {
                    _textChangedSubject.OnNext(new TextRecognitionResult(result.Text, true));
                }
            }
            else
            {
                var json = _recognizer.PartialResult();
                var partial = JsonSerializer.Deserialize<PartialRecognizerResult>(json);

                if (partial is not null)
                {
                    _textChangedSubject.OnNext(new TextRecognitionResult(partial.Partial, false));
                }
            }
        }

        private sealed record RecognizerResult([property: JsonPropertyName("text")] string Text);
        private sealed record PartialRecognizerResult([property: JsonPropertyName("partial")] string Partial);
    }
}
