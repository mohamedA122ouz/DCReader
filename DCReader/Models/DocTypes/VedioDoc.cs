
namespace DCReader.Models;


public class VedioDoc : IDoc
{
    public void Read(IFormFile file)
    {
        throw new NotImplementedException();
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
