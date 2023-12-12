using NAudio.Wave;
using Vosk;

var model = new Model("vosk-model-small-en-us-0.15");

var waveIn = new WaveInEvent(); // WasapiLoopbackCapture()
waveIn.WaveFormat = new WaveFormat(16000, 1);

var recognizer = new VoskRecognizer(model, waveIn.WaveFormat.SampleRate);

recognizer.SetMaxAlternatives(0);
recognizer.SetWords(true);

waveIn.DataAvailable += (s, a) =>
{
    if (recognizer.AcceptWaveform(a.Buffer, a.BytesRecorded))
    {
        Console.WriteLine(recognizer.Result());
    }
};

waveIn.StartRecording();

Console.WriteLine("Press return to stop transcribing");
Console.ReadLine();
