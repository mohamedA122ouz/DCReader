namespace DCReader.Models;

using System.Collections.Generic;
using DCReader.Models;
using Tesseract;

public class ImageDoc : IDoc
{
  private Trie trie;
  
    public List<Position> Search(string key)
    {
        return trie.search(key);
    }

    public Dictionary<string, List<Position>> SearchList(List<string> keys)
    {
        return trie.searchList(keys);
    }

    public void SetTrie(Trie trie)
    {
        this.trie=trie;
    }

    void IDoc.Read(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            Console.WriteLine("Invalid file");
            return;
        }
        string output = "";
        string extension = Path.GetExtension(file.FileName).ToLower();
        if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
        {
            Console.WriteLine("Unsupported file format");
            return;
        }
        MemoryStream ms = new();
        file.CopyTo(ms);

        byte[] fileBytes = ms.ToArray();
        ms.Close();
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        basePath = basePath.Substring(0, basePath.IndexOf("DCReader"));
        basePath = Path.Combine(basePath,"DCReader","tessdata");
        using (var engine = new TesseractEngine(basePath, "eng", EngineMode.Default))
        {
            using (var img = Pix.LoadFromMemory(fileBytes))
            {
                using (var page = engine.Process(img))
                {
                    output = page.GetText();
                }
            }
        }
        output = output.Trim();
        trie.insert(output);
    }
}