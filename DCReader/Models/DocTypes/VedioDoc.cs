namespace DCReader.Models;

using NAudio.Wave;
using System;
using System.IO;
using Microsoft.AspNetCore.Http;
using Vosk;
using System.Diagnostics;
using System.Collections.Generic;

public class VedioDoc : IDoc
{
  private static readonly string modelPath = "vosk-model-small-en-us";
  private Trie trie=new Trie();
 
public void SetTrie(Trie trie)
    {
        this.trie=trie;
    }
  public void Read(IFormFile videoFile)
  {
    if (videoFile == null || videoFile.Length == 0)
    {
      Console.Write("File Not Found");
    }

    string tempVideoPath = Path.GetTempFileName();
    using (var fileStream = new FileStream(tempVideoPath, FileMode.Create))
    {
      videoFile.CopyTo(fileStream);
    }

    string audioFilePath = ExtractAudio(tempVideoPath);

    // Convert Audio to WAV if necessary
    if (!audioFilePath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
    {
      audioFilePath = ConvertToWav(audioFilePath);
    }

    Vosk.SetLogLevel(0);
    var model = new Model(modelPath);

    string transcript = ConvertAudioToText(audioFilePath, model);

    File.Delete(tempVideoPath);
    if (audioFilePath != tempVideoPath) File.Delete(audioFilePath);
    trie.insert(transcript);
  }

  private string ExtractAudio(string videoPath)
  {
    string audioOutput = Path.ChangeExtension(videoPath, ".mp3");

    ProcessStartInfo psi = new ProcessStartInfo
    {
      FileName = "ffmpeg",
      Arguments = $"-i \"{videoPath}\" -q:a 0 -map a \"{audioOutput}\"",
      RedirectStandardOutput = true,
      RedirectStandardError = true,
      UseShellExecute = false,
      CreateNoWindow = true
    };

    using (Process process = new Process { StartInfo = psi })
    {
      process.Start();
      process.WaitForExit();
    }

    return audioOutput;
  }

  private string ConvertToWav(string inputFilePath)
  {
    string outputFilePath = Path.ChangeExtension(inputFilePath, ".wav");

    ProcessStartInfo psi = new ProcessStartInfo
    {
      FileName = "ffmpeg",
      Arguments = $"-i \"{inputFilePath}\" -acodec pcm_s16le -ar 16000 -ac 1 \"{outputFilePath}\"",
      RedirectStandardOutput = true,
      RedirectStandardError = true,
      UseShellExecute = false,
      CreateNoWindow = true
    };

    using (Process process = new Process { StartInfo = psi })
    {
      process.Start();
      process.WaitForExit();
    }

    return outputFilePath;
  }
  private string ConvertAudioToText(string audioFilePath, Model model)
  {
    using (var recognizer = new VoskRecognizer(model, 16000.0f))
    {
      using (var waveStream = new WaveFileReader(audioFilePath))
      {
        byte[] buffer = new byte[4096];
        int bytesRead;
        while ((bytesRead = waveStream.Read(buffer, 0, buffer.Length)) > 0)
        {
          recognizer.AcceptWaveform(buffer, bytesRead);
        }
      }
      return recognizer.FinalResult();
    }
  }

  public List<Position> Search(string key)
  {
    throw new NotImplementedException();
  }

  public Dictionary<string, List<Position>> SearchList(List<string> keys)
  {
    throw new NotImplementedException();
  }
}
