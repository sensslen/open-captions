// Copyright (c) Renewed Vision, LLC. All rights reserved.

using NAudio.CoreAudioApi;
using NAudio.Wave;
using Pro.LyricsBot.ViewModels;

namespace Pro.LyricsBot.Services.VoskTranscriber
{

    public class AudioSourceProvider : IAudioSourceProvider
    {
        public IList<ISelectionDescriptor> GetAvailable() =>
            new MMDeviceEnumerator().
                EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.All).
            Where(device => device.State == DeviceState.Active).
            Select(d => (ISelectionDescriptor)new WrappedAudioDevice(d)).ToList();

        public IWaveIn? Open(ISelectionDescriptor? description)
        {
            if (description is WrappedAudioDevice wrapped)
            {
                var result = new WasapiCapture(wrapped.Device);
                result.WaveFormat = new WaveFormat(16000, 1);

                result.StartRecording();
                return result;
            }
            return default;
        }

        private sealed record WrappedAudioDevice(MMDevice Device) : ISelectionDescriptor
        {
            public string Id => Device.ID;

            public string Name => Device.FriendlyName;
        }
    }
}
