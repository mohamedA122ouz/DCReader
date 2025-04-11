
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace DCReader.Models;

public class PDFDoc : TextDoc
{
    public override void Read(IFormFile file)
    {
        PdfDocument document = PdfDocument.Open(file.OpenReadStream());
        string pageText = "";
        foreach (Page page in document.GetPages())
        {
            var lines = page
                    .GetWords()
                    .GroupBy(w => Math.Round(w.BoundingBox.Bottom, 1)) // group by Y
                    .OrderByDescending(g => g.Key); // Top to bottom

            foreach (var lineGroup in lines) {
                string lineText = string.Join(" ", lineGroup.OrderBy(w => w.BoundingBox.Left).Select(w => w.Text));
                pageText += lineText +"\n"; // One line per line!
            }
        }
        trie.insert(pageText);
    }
}
