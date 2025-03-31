
namespace DCReader.Models;



public class TextDoc : IDoc
{
  private Trie trie=new Trie();
 
  public void SetTrie(Trie trie)
    {
        this.trie=trie;
    }
  public void Read(IFormFile file)
  {
    Stream stream = file.OpenReadStream();
    StreamReader reader = new(stream);
    int Row = 0;
    int Column = 0;
    int charCode;
    while ((charCode = reader.Read()) != -1)  // Read next character
    {
      char currentChar = (char)charCode;
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
  public List<Position> Search(string key)
  {
    return trie.search(key);
  }
  public Dictionary<string, List<Position>> SearchList(List<string> keys)
  {
    return trie.searchList(keys);
  }
}
