// Copyright (c) Renewed Vision, LLC. All rights reserved.

using System.Reflection;

namespace Pro.LyricsBot.Services
{
    public class ModelServiceProvider : IModelServiceProvider
    {
        public IEnumerable<IVoskModelDescriptor> Available()
        {
            var assemblypath = Assembly.GetExecutingAssembly().Location;
            var containingFolder = Path.GetDirectoryName(assemblypath);
            foreach (var folder in Directory.GetDirectories(Path.Combine(containingFolder ?? string.Empty, "models")))
            {
                yield return new ModelDescriptor(folder);
            }
        }
        public Vosk.Model Get(IVoskModelDescriptor descriptor)
        {
            if (descriptor is ModelDescriptor modelInfo)
            {
                return new Vosk.Model(modelInfo.FullPath);
            }
            throw new ArgumentException($"{descriptor.GetType()} not supported");
        }

        private sealed record ModelDescriptor(string FullPath) : IVoskModelDescriptor
        {
            public string Name => Path.GetFileName(FullPath);
            public string Id => FullPath;
        }
    }
}
