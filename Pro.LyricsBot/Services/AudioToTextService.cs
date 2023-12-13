using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pro.LyricsBot.Services
{

    internal class AudioToTextService : IAudioToTextService
    {
        public IObservable<string> WhenRecognizedTextChanged => throw new NotImplementedException();

        public IObservable<string> WhenRecognitionEnded => throw new NotImplementedException();
    }
}
