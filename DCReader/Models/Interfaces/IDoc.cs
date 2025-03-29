namespace DCReader.Models;

public interface IDoc{
  const string Error="File Not Found";
  public string Read(IFormFile file);
  public void Search();
}
