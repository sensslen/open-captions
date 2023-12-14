// Copyright (c) Renewed Vision, LLC. All rights reserved.

using NAudio.CoreAudioApi;
using NAudio.Wave;
using Pro.LyricsBot.ViewModels;

namespace Pro.LyricsBot.Services.VoskTranscriber
{

    public class AudioSourceProvider : IAudioSourceProvider
    {
        public IList<ISelectionDescriptor> GetAvailable()
        {
            var enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active).
                Select(d => (ISelectionDescriptor)new WrappedAudioDevice(d));
            return devices.Concat(enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).
                Select(d => (ISelectionDescriptor)new LoopbackAudioDevice(d))).ToList();
        }

        public IWaveIn? Open(ISelectionDescriptor? description)
        {
            if (description is WrappedAudioDevice wrapped)
            {
                var result = new WasapiCapture(wrapped.Device);
                result.WaveFormat = new WaveFormat(16000, 1);

                result.StartRecording();
                return result;
            }
            else if (description is LoopbackAudioDevice loopback)
            {
                var result = new WasapiLoopbackCapture(loopback.Device);
                result.WaveFormat = new WaveFormat(16000, 1);

                result.StartRecording();
                return result;
            }
            return default;
        }

        private sealed record LoopbackAudioDevice(MMDevice Device) : ISelectionDescriptor
        {
            public string Id => $"Loopback {Device.ID}";

            public string Name => $"--Loopback-- {Device.FriendlyName}";
        }

        private sealed record WrappedAudioDevice(MMDevice Device) : ISelectionDescriptor
        {
            public string Id => Device.ID;

            public string Name => Device.FriendlyName;
        }
    }
}
