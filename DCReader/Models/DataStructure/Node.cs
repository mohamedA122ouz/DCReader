namespace DCReader.Models;
struct Node{
    public char Value;
    public int[] columnIndex;
    public int[] RowIndex;
    public bool[] IsFullWord;
    public Dictionary<char,Node> next;
}
record Position {public int Column;public int Row;}