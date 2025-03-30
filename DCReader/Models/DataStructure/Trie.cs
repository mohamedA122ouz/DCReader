namespace DCReader.Models;

public class Trie
{
    private char startCharValue = '&';
    private Node startNode { get; set; }
    private Node lastNode { get; set; }
    private Node IntiateNode(char charValue)
    {
        Node tempNode = new();
        tempNode.Value = charValue;
        tempNode.next = new();
        tempNode.RowIndex = new();
        tempNode.columnIndex = new();
        tempNode.IsFullWord = new();
        return tempNode;
    }
    public Trie()
    {
        startNode = IntiateNode(startCharValue);
        lastNode = startNode;
    }
    public Node insert(char i, Position charPosition)
    {
        if (i == ' ')
        {
            if (lastNode.IsFullWord.Count != 0)
                lastNode.IsFullWord[lastNode.IsFullWord.Count - 1] = true;

            lastNode = startNode;
            return new();
        }
        Node current = lastNode.next.GetValueOrDefault(i);
        if (current.Value == '\0')
        {
            current = IntiateNode(i);
        }

        current.columnIndex.Add(charPosition.Column);
        current.RowIndex.Add(charPosition.Row);
        current.IsFullWord.Add(false);
        lastNode.next.TryAdd(i, current);
        lastNode = current;
        return current;
    }
    public bool contains(string key){
        Node tempNode = startNode;
        foreach (char c in key)
        {
            Node nextTemp = tempNode.next.GetValueOrDefault(c);
            if (nextTemp.Value == '\0')
                return false;
            tempNode = nextTemp;
        }
        return true;
    }
    public List<Position> search(string key)
    {
        List<Position> positions = new();
        Node tempNode = startNode;
        int i = 0;
        foreach (char c in key)
        {
            Node nextTemp = tempNode.next.GetValueOrDefault(c);
            if (nextTemp.Value == '\0')
                break;
            tempNode = nextTemp;
            i++;
            if(i == key.Length - 1){
                for(int ii = 0;ii<tempNode.RowIndex.Count;ii++){
                    positions.Add(new(){Column=tempNode.columnIndex[ii] - key.Length + 2,Row=tempNode.RowIndex[ii]});
                }
            }
        }
        return positions;
    }
    public Dictionary<string,List<Position>> searchList(List<string> keys){
        Dictionary<string,List<Position>> searchItems = new();
        foreach(string key in keys){
            searchItems.Add(key,search(key));
        }
        return searchItems;
    }
}