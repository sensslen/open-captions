// Copyright (c) Renewed Vision, LLC. All rights reserved.

using NAudio.Wave;

namespace Pro.LyricsBot.Services
{
    public interface IAudioSourceProvider
    {
        IEnumerable<IAudioDeviceDescription> GetAvailable();

        IWaveIn Open(IAudioDeviceDescription description);
    }
}
