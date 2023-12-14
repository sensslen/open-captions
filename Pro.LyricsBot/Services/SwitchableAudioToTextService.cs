using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Pro.LyricsBot.Services
{
    public class SwitchableAudioToTextService : DisposableBase, ISwitchableAudioToTextService
    {
        private readonly BehaviorSubject<IAudioToTextService?> _audioToTextServiceSubject = new BehaviorSubject<IAudioToTextService?>(default);

        public IObservable<TextRecognitionResult> WhenTextChanged => _audioToTextServiceSubject.Select(GetTextChangeObservable).Switch();

        protected override void OnDispose()
        {
            _audioToTextServiceSubject.Value?.Dispose();
            _audioToTextServiceSubject.OnCompleted();
        }

        public void Switch(IAudioToTextService audioToTextService)
        {
            _audioToTextServiceSubject.Value?.Dispose();
            _audioToTextServiceSubject.OnNext(audioToTextService);
        }

        public IObservable<TextRecognitionResult> GetTextChangeObservable(IAudioToTextService? service)
        {
            return service?.WhenTextChanged ?? Observable.Empty<TextRecognitionResult>();
        }
    }
}
