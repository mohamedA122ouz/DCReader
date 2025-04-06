namespace DCReader.Models;

public class Trie
{
    private char startCharValue = '␀';
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
    public void insert(string wholeString)
    {
        int Row = 0;
        int Column = 0;
        int strCounter = 0;
        while (strCounter < wholeString.Length)
        {
            char currentChar = wholeString[strCounter++];
            if (currentChar == '\n')
            {
                Column = 0;
                Row++;
                lastNode = startNode;
                continue;
            }
            if (currentChar == '\r' || currentChar == '\t'){
                lastNode = startNode;
                Column++;
                continue;
            }
            insert(currentChar, new() { Row = Row, Column = Column++ });
        }
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
    public bool contains(string key)
    {
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
        int i = 0;//char c counter
        foreach (char c in key)
        {
            Node nextTemp = tempNode.next.GetValueOrDefault(c);
            if (nextTemp.Value == '\0')
                break;
            tempNode = nextTemp;
            if (i == key.Length - 1)
            {
                for (int ii = 0; ii < tempNode.RowIndex.Count; ii++)
                {
                    positions.Add(new() { Column = tempNode.columnIndex[ii] - key.Length + 1, Row = tempNode.RowIndex[ii] });
                }
            }
            i++;
        }
        return positions;
    }
    public Dictionary<string, List<Position>> searchList(List<string> keys)
    {
        Dictionary<string, List<Position>> searchItems = new();
        foreach (string key in keys)
        {
            if(!searchItems.ContainsKey(key))
                searchItems.Add(key, search(key));
        }
        return searchItems;
    }
}