namespace DCReader.Models;
using DCReader.Models;

public class Document{
  private IDoc Doc ;
  private Trie trie=new Trie();


  public Document(IDoc doc){
    Doc=doc;

  }
  public void read(IFormFile file){
    Doc.SetTrie(trie);
    Doc.Read(file);
  }
  public List<Position> Search(string key)
  {
    return trie.search(key);
  }
  public Dictionary<string, List<Position>> SearchList(List<string> keys)
  {
    return trie.searchList(keys);
  }  public void ChangeState(IDoc doc){
    Doc=doc;
  }
}

