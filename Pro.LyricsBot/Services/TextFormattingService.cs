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
            WhenTextChanged = audioToTextService.WhenTextChanged.Scan(new TextFormattingState(Array.Empty<string>(), Array.Empty<string>()), GenerateFormattedText).Select(MergeLines);
        }

        private string MergeLines(TextFormattingState source) => string.Join(Environment.NewLine, source.lines.Concat(source.partialLines).TakeLast(_settings.LineCount));

        private TextFormattingState GenerateFormattedText(TextFormattingState state, TextRecognitionResult result)
        {
            var addedLines = GetLines(result.text);
            if (result.isEnd)
            {
                return new TextFormattingState(state.lines.Concat(addedLines).TakeLast(_settings.LineCount).ToList(), Array.Empty<string>());
            }
            else
            {
                return state with { partialLines = addedLines };
            }
        }

        private IList<string> GetLines(string text)
        {
            var words = text.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
            var result = new List<string>();
            var line = new StringBuilder();

            foreach (var word in words)
            {
                if (line.Length > 0 && line.Length + word.Length + 1 > _settings.LineLength)
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

        private record TextFormattingState(IList<string> lines, IList<string> partialLines);
    }
}
