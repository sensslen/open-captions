// Copyright (c) Renewed Vision, LLC. All rights reserved.

using System.ComponentModel;

namespace Pro.LyricsBot.ViewModels.Models
{
    public interface IVoskModelSettingsVM : IModelSettingsVM, INotifyPropertyChanged
    {
        IList<ISelectionDescriptor> AvailableModels { get; }
        ISelectionDescriptor? SelectedModel { get; set; }
        IList<ISelectionDescriptor> AvailableAudioSources { get; }
        ISelectionDescriptor? SelectedAudioSource { get; set; }
    }
}
