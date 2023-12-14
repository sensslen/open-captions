// Copyright (c) Renewed Vision, LLC. All rights reserved.

using System.Reflection;
using Pro.LyricsBot.ViewModels;

namespace Pro.LyricsBot.Services.VoskTranscriber
{
    public class VoskModelProvider : IVoskModelProvider
    {
        public IList<ISelectionDescriptor> GetAvailable()
        {
            var assemblypath = Assembly.GetExecutingAssembly().Location;
            var containingFolder = Path.GetDirectoryName(assemblypath);
            return Directory.GetDirectories(Path.Combine(containingFolder ?? string.Empty, "models")).
                Select(m => (ISelectionDescriptor)new ModelDescriptor(m)).ToList();

        }
        public Vosk.Model? Create(ISelectionDescriptor? descriptor)
        {
            if (descriptor is ModelDescriptor modelInfo)
            {
                return new Vosk.Model(modelInfo.FullPath);
            }
            return null;
        }

        private sealed record ModelDescriptor(string FullPath) : ISelectionDescriptor
        {
            public string Name => Path.GetFileName(FullPath);
            public string Id => FullPath;
        }
    }
}
