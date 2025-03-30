namespace DCReader.Models;

using System.Collections.Generic;
using DCReader.Models;
public class ImageDoc:IDoc{

    public List<Position> Search(string key)
    {
        throw new NotImplementedException();
    }

    public Dictionary<string, List<Position>> SearchList(List<string> keys)
    {
        throw new NotImplementedException();
    }

    void IDoc.Read(IFormFile file)
    {
        throw new NotImplementedException();
    }
} 
