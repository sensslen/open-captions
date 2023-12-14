// Copyright (c) Renewed Vision, LLC. All rights reserved.

using Pro.LyricsBot.ViewModels;

namespace Pro.LyricsBot.Services.VoskTranscriber
{
    public class VoskAudioToTextFactory : IVoskAudioToTextFactory
    {
        private readonly IVoskModelProvider _voskModelProvider;
        private readonly IAudioSourceProvider _audioSourceProvider;
        private readonly ISwitchableAudioToTextService _switchableAudioToTextService;

        public VoskAudioToTextFactory(IVoskModelProvider voskModelProvider, IAudioSourceProvider audioSourceProvider, ISwitchableAudioToTextService switchableAudioToTextService)
        {
            _voskModelProvider = voskModelProvider;
            _audioSourceProvider = audioSourceProvider;
            _switchableAudioToTextService = switchableAudioToTextService;
        }

        public void Switch(ISelectionDescriptor? selectedModel, ISelectionDescriptor? selectedAudioInterface)
        {
            var model = _voskModelProvider.Create(selectedModel);
            var audioInterface = _audioSourceProvider.Open(selectedAudioInterface);
            if (model is not null && audioInterface is not null)
            {
                var newService = new VoskAudioToTextService(model, audioInterface);
                _switchableAudioToTextService.Switch(newService);
            }
        }
    }
}
