namespace DCReader.Models;

using System.Collections.Generic;
using DCReader.Models;
using Tesseract;

public class ImageDoc : IDoc
{
    private Trie trie { get; set; } = new();

    public List<Position> Search(string key)
    {
        return trie.search(key);
    }

    public Dictionary<string, List<Position>> SearchList(List<string> keys)
    {
        return trie.searchList(keys);
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
        if (extension != ".png" || extension != ".jpg" || extension != ".jpeg")
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

        int Row = 0;
        int Column = 0;
        int strCounter = 0;
        while (strCounter < output.Length)
        {
            char currentChar = output[strCounter++];
            if (currentChar == '\n')
            {
                Column = 0;
                Row++;
                continue;
            }
            if (currentChar == '\r' || currentChar == '\t')
                continue;
            trie.insert(currentChar, new() { Row = Row, Column = Column++ });
        }
    }
}

