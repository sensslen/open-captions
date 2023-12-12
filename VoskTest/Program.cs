using NAudio.Wave;
using Vosk;


var model = new Model("vosk-model-small-en-us-0.15");

var capture = new WasapiLoopbackCapture();
var recognizer = new VoskRecognizer(model, capture.WaveFormat.SampleRate);

recognizer.SetMaxAlternatives(0);
recognizer.SetWords(true);

capture.DataAvailable += (s, a) =>
{
    if (recognizer.AcceptWaveform(a.Buffer, a.BytesRecorded))
    {
        Console.WriteLine(recognizer.Result());
    }
};

