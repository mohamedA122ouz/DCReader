namespace DCReader.Models;

public interface IDoc{
  const string Error="File Not Found";
  public void Read(IFormFile file);
  public List<Position> Search(string key);
  public Dictionary<string,List<Position>> SearchList(List<string> keys);
  public void SetTrie(Trie trie);
}
