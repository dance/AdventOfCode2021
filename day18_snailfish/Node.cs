namespace day18_snailfish;

public enum NodeType
{
    Pair,
    RegularNumber
}

public class Node
{
    public NodeType Type;
    public Node? Left;
    public Node? Right;
    public int Value { get; set; }

    public Node(Node left, Node right)
    {
        Type = NodeType.Pair;
        Left = left;
        Right = right;
    }
    
    public Node(int value)
    {
        Type = NodeType.RegularNumber;
        Value = value;
    }

    public bool IsReqularNumber => Type == NodeType.RegularNumber;

    public bool IsPair => Type == NodeType.Pair;

    public void ConvertToRegularNumber()
    {
        if (Type != NodeType.Pair)
            throw new Exception();
        Left = null;
        Right = null;
        Value = 0;
        Type = NodeType.RegularNumber;
    }

    public void Split()
    {
        if (Type != NodeType.RegularNumber)
            throw new Exception();
        Type = NodeType.Pair;
        int leftValue = Value / 2;
        Left = new Node(leftValue);
        Right = new Node(Value - leftValue);
    }

    public override string ToString()
    {
        return Type == NodeType.Pair ? "Pair" : $"Regular {Value}";
    }
}
