﻿// Copyright (c) Renewed Vision, LLC. All rights reserved.

using NAudio.Wave;
using Pro.LyricsBot.ViewModels;

namespace Pro.LyricsBot.Services.VoskTranscriber
{
    public interface IAudioSourceProvider
    {
        IList<ISelectionDescriptor> GetAvailable();

        IWaveIn? Open(ISelectionDescriptor? description);
    }
}
