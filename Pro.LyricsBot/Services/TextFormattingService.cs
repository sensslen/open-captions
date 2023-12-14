// Copyright (c) Renewed Vision, LLC. All rights reserved.

using System.Reactive.Linq;
using System.Text;

namespace Pro.LyricsBot.Services
{
    internal class TextFormattingService : ITextFormattingService
    {
        private readonly ISettings _settings;
        public IObservable<string> WhenTextChanged { get; }

        public TextFormattingService(IAudioToTextService audioToTextService, ISettings settings)
        {
            _settings = settings;
            WhenTextChanged = audioToTextService.WhenTextChanged.Scan(new TextFormattingState(Array.Empty<Line>(), Array.Empty<Line>()), GenerateFormattedText).Select(MergeLines);
        }

        private string MergeLines(TextFormattingState source) => string.Join(Environment.NewLine, source.lines.Concat(source.partialLines).TakeLast(_settings.LineCount).Select(line => { return line.text; }));

        private TextFormattingState GenerateFormattedText(TextFormattingState state, TextRecognitionResult result)
        {
            var addedLines = GetLines(result.text);
            if (result.isEnd)
            {
                return new TextFormattingState(state.lines.Concat(addedLines).TakeLast(_settings.LineCount).Where(line => { var age = DateTime.Now - line.timestamp; return (age.Seconds < 5); }).ToList(), Array.Empty<Line>());
            }
            else
            {
                return state with { partialLines = addedLines };
            }
        }

        private IList<Line> GetLines(string text)
        {
            var words = text.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
            var result = new List<Line>();
            var line = new StringBuilder();

            foreach (var word in words)
            {
                if (line.Length > 0 && line.Length + word.Length + 1 > _settings.LineLength)
                {
                    result.Add(new Line(line.ToString(), DateTime.Now));
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
                result.Add(new Line(line.ToString(), DateTime.Now));
            }

            return result;
        }

        private record Line(string text, DateTime timestamp);
        private record TextFormattingState(IList<Line> lines, IList<Line> partialLines);
    }
}
