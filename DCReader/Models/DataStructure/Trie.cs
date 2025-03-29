namespace DCReader.Models;

public class Trie
{
    private char startCharValue = '&';
    private Node startNode { get; set; }
    private Node lastNode { get; set; }
    private Node IntiateNode(char charValue){
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
}