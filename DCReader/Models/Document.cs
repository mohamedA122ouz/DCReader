namespace DCReader.Models;
using DCReader.Models;

public class Document{
  private IDoc Doc ;


  public Document(IDoc doc){
    Doc=doc;

  }
  public string read(IFormFile file){
    return Doc.Read(file);
  }
}

