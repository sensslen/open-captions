using NAudio.Wave;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using Vosk;

namespace Pro.LyricsBot.Services
{
    internal class AudioToTextService : IAudioToTextService
    {
        private readonly SubjectBase<string> _textChangedSubject = new ReplaySubject<string>(1);
        private readonly SubjectBase<string> _recognitionEndedSubject = new ReplaySubject<string>(1);
        private readonly VoskRecognizer _recognizer;
        private readonly IWaveIn _audioStream;
        private bool disposedValue;

        public IObservable<string> WhenRecognizedTextChanged => _textChangedSubject.DistinctUntilChanged();

        public IObservable<string> WhenRecognitionEnded => _recognitionEndedSubject;

        public AudioToTextService(string pathToRecognitionModel, IWaveIn audioStream)
        {
            _audioStream = audioStream;

            var model = new Model(pathToRecognitionModel);
            _recognizer = new VoskRecognizer(model, audioStream.WaveFormat.SampleRate);
            _recognizer.SetMaxAlternatives(0);
            _recognizer.SetWords(true);

            _audioStream.DataAvailable += AudioDataReceived;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _audioStream.DataAvailable -= AudioDataReceived;
                    _recognizer.Dispose();
                }

                disposedValue = true;
            }
        }

        private void AudioDataReceived(object? sender, WaveInEventArgs e)
        {

            if (_recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
            {
                var json = _recognizer.Result();
                var result = JsonSerializer.Deserialize<RecognizerResult>(json);
                if (result is not null)
                {
                    _recognitionEndedSubject.OnNext(result.Text);
                }
            }
            else
            {
                var json = _recognizer.PartialResult();
                var partial = JsonSerializer.Deserialize<PartialRecognizerResult>(json);

                if (partial is not null)
                {
                    _textChangedSubject.OnNext(partial.Partial);
                }
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public record RecognizerResult(string Text);
    public record PartialRecognizerResult(string Partial);
}
