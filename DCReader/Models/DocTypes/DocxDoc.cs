using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text;

namespace DCReader.Models;
public class DocxDoc:TextDoc {
    public override void Read(IFormFile file) {
        string sb = "";
        WordprocessingDocument wordDoc = WordprocessingDocument.Open(file.OpenReadStream(),false);
        Body body = wordDoc.MainDocumentPart.Document.Body;
        foreach (var para in body.Elements<Paragraph>()) {
            string paragraphText = string.Join("", para.Descendants<Text>().Select(t => t.Text));
            sb += paragraphText + "\n";
        }
        
        trie.insert(sb);
    }
}

