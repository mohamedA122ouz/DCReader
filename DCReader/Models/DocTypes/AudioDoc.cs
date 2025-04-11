namespace DCReader.Models;

using NAudio.Wave;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Vosk;
using System.Diagnostics;
using System.Collections.Generic;

public class AudioDoc : IDoc
{
  private static readonly string modelPath = "vosk-model-small-en-us";

  private Trie trie;

  public void SetTrie(Trie trie)
    {
        this.trie=trie;
    }
  public void Read(IFormFile audioFile)
  {
    if (audioFile == null || audioFile.Length == 0)
    {
      Console.WriteLine("File Not Found");
    }

    // Save uploaded file to a temp location
    string tempFilePath = Path.GetTempFileName();
    using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
    {
      audioFile.CopyTo(fileStream);
    }

    string wavFilePath = ConvertToWav(tempFilePath);

    Vosk.SetLogLevel(0);
    var model = new Model(modelPath);

    string transcript = ConvertAudioToText(wavFilePath, model);

    File.Delete(tempFilePath);
    if (wavFilePath != tempFilePath) File.Delete(wavFilePath);

    trie.insert(transcript);
    Console.WriteLine(transcript);
  }

  public List<Position> Search(string key)
  {
    return trie.search(key);  
  }

  public Dictionary<string, List<Position>> SearchList(List<string> keys)
  {
    //predefind within the Trie datastructure 
    //only pass the keys to it and return the result then done
    return trie.searchList(keys);
  }

  private string ConvertAudioToText(string filePath, Model model)
  {
    using var waveStream = new WaveFileReader(filePath);
    using var recognizer = new VoskRecognizer(model, waveStream.WaveFormat.SampleRate);

    byte[] buffer = new byte[4096];
    int bytesRead;
    while ((bytesRead = waveStream.Read(buffer, 0, buffer.Length)) > 0)
    {
      recognizer.AcceptWaveform(buffer, bytesRead);
    }

    return recognizer.FinalResult();
  }


  private string ConvertToWav(string inputFilePath)
  {
    string outputFilePath = Path.ChangeExtension(inputFilePath, ".wav");

    ProcessStartInfo processStartInfo = new ProcessStartInfo
    {
      FileName = "ffmpeg",
      Arguments = $"-i \"{inputFilePath}\" -acodec pcm_s16le -ar 16000 -ac 1 \"{outputFilePath}\"",
      RedirectStandardOutput = true,
      RedirectStandardError = true,
      UseShellExecute = false,
      CreateNoWindow = true
    };

    using (Process process = new Process { StartInfo = processStartInfo })
    {
      process.Start();
      process.WaitForExit();
    }

    return outputFilePath;
  }
}

