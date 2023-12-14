// Copyright (c) Renewed Vision, LLC. All rights reserved.

using Pro.LyricsBot.ViewModels;

namespace Pro.LyricsBot.Services.VoskTranscriber
{
    public interface IVoskModelProvider
    {
        IList<ISelectionDescriptor> GetAvailable();
        Vosk.Model? Create(ISelectionDescriptor? descriptor);
    }
}
