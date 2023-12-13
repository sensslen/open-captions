// Copyright (c) Renewed Vision, LLC. All rights reserved.

namespace Pro.LyricsBot.Services
{
    public interface ITextFormattingService : IDisposable
    {
        IObservable<string> WhenTextChanged { get; }
    }
}
