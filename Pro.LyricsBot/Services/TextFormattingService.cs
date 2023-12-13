// Copyright (c) Renewed Vision, LLC. All rights reserved.

using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace Pro.LyricsBot.Services
{
    internal class TextFormattingService : DisposableBase, ITextFormattingService
    {
        private readonly SubjectBase<string> _textChangedSubject = new ReplaySubject<string>(1);
        private IAudioToTextService _audioToTextService;
        private IDisposable _recognitionEndedSubscription;
        private IDisposable _recognizedTextChangedSubscription;
        private List<string> _previousLines = new List<string>();
        private string _partialText = "";

        public int LineCount { get; set; }
        public int LineLength { get; set; }

        public TextFormattingService(IAudioToTextService audioToTextService)
        {
            _audioToTextService = audioToTextService;
            _recognitionEndedSubscription = _audioToTextService.WhenRecognitionEnded.Subscribe(AddFinalText);
            _recognizedTextChangedSubscription = _audioToTextService.WhenRecognizedTextChanged.Subscribe(UpdatePartialText);
        }

        public IObservable<string> WhenTextChanged => _textChangedSubject.DistinctUntilChanged();

        private List<string> LineBreak(string text)
        {
            var result = new List<string>();
            var line = new StringBuilder();

            foreach (var word in text.Split(' '))
            {
                if (line.Length > 0 && line.Length + word.Length + 1 > LineLength)
                {
                    result.Add(line.ToString());
                    line.Clear();
                }
                else if (line.Length > 0)
                {
                    line.Append(" ");
                }
                line.Append(word);
            }

            if (line.Length > 0)
            {
                result.Add(line.ToString());
            }

            return result;
        }

        private void FormatText()
        {
            while (_previousLines.Count > LineCount)
            {
                _previousLines.RemoveAt(0);
            }

            var lines = new List<string>();
            foreach (var line in _previousLines)
            {
                lines.AddRange(LineBreak(line));
            }
            if (_partialText.Length > 0)
            {
                lines.AddRange(LineBreak(_partialText));
            }

            var output = new StringBuilder();
            for (int i = LineCount; i > 0; --i)
            {
                output.AppendLine(lines[^i]);
            }

            _textChangedSubject.OnNext(output.ToString());
        }

        private void UpdatePartialText(string text)
        {
            _partialText = text;
            FormatText();
        }

        private void AddFinalText(string text)
        {
            _partialText = "";
            if (text.Length > 0)
            {
                _previousLines.Add(text);
            }
            FormatText();
        }

        protected override void OnDispose()
        {
            _recognizedTextChangedSubscription.Dispose();
            _recognizedTextChangedSubscription.Dispose();
        }
    }
}
