// Copyright (c) Renewed Vision, LLC. All rights reserved.

namespace Pro.LyricsBot.Services
{
    public interface ITextFormattingService
    {
        IObservable<string> WhenTextChanged { get; }
    }
}
