namespace DCReader.Models;
using DCReader.Models;

public class Document{
  private IDoc Doc ;


  public Document(IDoc doc){
    Doc=doc;

  }
  public void read(IFormFile file){
    Doc.Read(file);
  }
}

