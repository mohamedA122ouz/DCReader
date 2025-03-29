namespace DCReader.Models;
using DCReader.Models;

class Trie
{
    private char startCharValue = '&';
    private Node startNode { get; set; }
    private Node lastNode { get; set; }
    private bool IsStarted { get; set; } = false;
    public Trie()
    {
        Node tempNode = new();
        tempNode.Value = startCharValue;
        startNode = tempNode;
        lastNode = startNode;
    }
    public Node insert(char i, Position charPosition, Node needed)
    {
        if (i == ' ')
        {
            if (IsStarted)
            {
                lastNode.IsFullWord[lastNode.IsFullWord.Length - 1] = true;
                lastNode = startNode;
            }
            return new();
        }
        Node current = needed.next.GetValueOrDefault(i);
        if (current.Value == '\0')
            current.Value = i;

        current.columnIndex.Append(charPosition.Column);
        current.RowIndex.Append(charPosition.Row);
        lastNode.IsFullWord.Append(false);
        startNode.next.Add(i, current);
        lastNode = current;
        if (!IsStarted)
            IsStarted = true;
        return current;
    }

}