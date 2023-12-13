// Copyright (c) Renewed Vision, LLC. All rights reserved.

using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace Pro.LyricsBot.Services
{

    public class AudioSourceProvider : IAudioSourceProvider
    {
        public IEnumerable<IAudioDeviceDescription> GetAvailable()
        {
            var enumerator = new MMDeviceEnumerator();
            foreach (MMDevice device in enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.All))
            {
                if (device.State == DeviceState.Active)
                {
                    yield return new WrappedAudioDevice(device);
                }
            }
        }
        public IWaveIn Open(IAudioDeviceDescription description)
        {
            if (description is WrappedAudioDevice wrapped)
            {
                return new WasapiLoopbackCapture(wrapped.Device);
            }
            throw new ArgumentException($"{description.GetType()} not supported");
        }

        private sealed record WrappedAudioDevice(MMDevice Device) : IAudioDeviceDescription
        {
            public string Id => Device.ID;

            public string Name => Device.FriendlyName;
        }
    }
}
