namespace DCReader.Models;
public struct Node{
    public char Value;
    public List<int> columnIndex;
    public List<int> RowIndex;
    public List<bool> IsFullWord;
    public Dictionary<char,Node> next;
}
public record Position {public int Column {get;set;} public int Row {get;set;}}