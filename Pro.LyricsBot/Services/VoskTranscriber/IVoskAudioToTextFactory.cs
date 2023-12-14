// Copyright (c) Renewed Vision, LLC. All rights reserved.

using Pro.LyricsBot.ViewModels;

namespace Pro.LyricsBot.Services.VoskTranscriber
{
    public interface IVoskAudioToTextFactory
    {
        void Switch(ISelectionDescriptor? selectedModel, ISelectionDescriptor? selectedAudioInterface);
    }
}
