using System.Net.Http.Json;
using System.Text.Json;
using NAudio.Wave;
using Vosk;

var host = "http://localhost:32475/";
var messageId = "0";

var httpClient = new HttpClient();

var previousPartial = "";
var red = "\x1b[31m";
var resetColor = "\x1b[0m";
var green = "\x1b[32m";

var messageGetter = httpClient.GetAsync($"{host}v1/message/{messageId}");
messageGetter.Wait();
var message = JsonSerializer.Deserialize<Message>(messageGetter.Result.Content.ReadAsStream());

Console.WriteLine(message.message);

var model = new Model("vosk-model-en-us-0.22-lgraph");

var waveIn = new WaveInEvent(); // WasapiLoopbackCapture()
waveIn.WaveFormat = new WaveFormat(16000, 1);

var recognizer = new VoskRecognizer(model, waveIn.WaveFormat.SampleRate);

recognizer.SetMaxAlternatives(0);
recognizer.SetWords(true);

waveIn.DataAvailable += (s, a) =>
{
    if (recognizer.AcceptWaveform(a.Buffer, a.BytesRecorded))
    {
        var json = recognizer.Result();
        var result = JsonSerializer.Deserialize<RecognizerResult>(json);
        // Console.WriteLine(json);
        Console.WriteLine($"{red}{result?.text}{resetColor}");
        SendToProAsync(result?.text);
    }
    else
    {
        var json = recognizer.PartialResult();
        var partial = JsonSerializer.Deserialize<RecognizerPartial>(json);
        if (partial != null && partial.partial.Length > 0)
        {
            if (partial.partial != previousPartial)
            {
                Console.WriteLine($"{green}{partial.partial}{resetColor}");
                SendToProAsync(partial.partial);
                previousPartial = partial.partial;
            }
        }
    }
};

waveIn.StartRecording();

Console.WriteLine("Press return to stop transcribing");
Console.ReadLine();

async Task SendToProAsync(string? text)
{
    message.message = text;
    var content = JsonContent.Create<Message>(message);
    await httpClient.PutAsync($"{host}v1/message/{messageId}", content);
    var triggerContent = JsonContent.Create(new List<Token>().ToArray());
    await httpClient.PostAsync($"{host}v1/message/{messageId}/trigger", triggerContent);
}

public class RecognizerResult
{
    public string? text { get; set; }
};

public class RecognizerPartial
{
    public string? partial { get; set; }
};

public class Id
{
    public int index { get; set; }
    public string? name { get; set; }
    public string? uuid { get; set; }
};

public class Token
{

};

public class Message
{
    public Id id { get; set; }
    public string? message { get; set; }
    public Token[] tokens { get; set; }
    public Id theme { get; set; }
    public bool visible_on_network { get; set; }
};
