using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Pro.LyricsBot.Services
{
    public class SwitchableAudioToTextService : DisposableBase, ISwitchableAudioToTextService
    {
        private readonly BehaviorSubject<IAudioToTextService?> _audioToTextServiceSubject = new BehaviorSubject<IAudioToTextService?>(default);

        public IObservable<TextRecognitionResult> WhenTextChanged => _audioToTextServiceSubject.Where(s => s is not null).Select(s => s!.WhenTextChanged).Switch();

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
    }
}
