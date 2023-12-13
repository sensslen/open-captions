// Copyright (c) Renewed Vision, LLC. All rights reserved.

namespace Pro.LyricsBot.Services
{
    public interface IModelServiceProvider
    {
        IEnumerable<IVoskModelDescriptor> Available();
        Vosk.Model Get(IVoskModelDescriptor descriptor);
    }
}
