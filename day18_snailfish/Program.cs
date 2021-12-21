// See https://aka.ms/new-console-template for more information

// tests

using System.Diagnostics;
using day18_snailfish;

// basic parsing and magnitude
// Solve("[1,2]");
// Solve("[[1,2],3]");
// Solve("[9,[8,7]]");
// Solve("[[1,2],[[3,4],5]]"); // 143
// Solve("[[[[0,7],4],[[7,8],[6,0]]],[8,1]]"); // 1384
// Solve("[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]"); // 3488

// explodes
// Solve("[[[[[9,8],1],2],3],4]");
// Solve("[7,[6,[5,[4,[3,2]]]]]");
// Solve("[[6,[5,[4,[3,2]]]],1]");

// splits

// combined tests: addition, reducing, magnitude
// Solve(@"
// [[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
// [[[5,[2,8]],4],[5,[[9,9],0]]]
// [6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
// [[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
// [[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
// [[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
// [[[[5,4],[7,7]],8],[[8,3],8]]
// [[9,3],[[9,9],[6,[4,9]]]]
// [[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
// [[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]"); // 4140

// input
Solve(File.ReadAllText("input.txt"));

// SolvePart2(@"
// [[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]
// [[[5,[2,8]],4],[5,[[9,9],0]]]
// [6,[[[6,2],[5,6]],[[7,6],[4,7]]]]
// [[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]
// [[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]
// [[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]
// [[[[5,4],[7,7]],8],[[8,3],8]]
// [[9,3],[[9,9],[6,[4,9]]]]
// [[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]
// [[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]");
SolvePart2(File.ReadAllText("input.txt"));

static void Solve(string input)
{
    string[] numbers = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    int numIdx = 0;
    var snailfishNumber = ReadNumber(numbers[numIdx++].GetEnumerator());
    Reduce(snailfishNumber);
    while (numIdx < numbers.Length)
    {
        var number = ReadNumber(numbers[numIdx++].GetEnumerator());
        snailfishNumber = new Node(snailfishNumber, number);
        Reduce(snailfishNumber);
    } 

    int magnitude = CalculateMagnitude(snailfishNumber);
    Console.WriteLine($"Magnitude is {magnitude}");
}

static void SolvePart2(string input)
{
    string[] numbers = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    int maxMagnitude = 0;
    for (int i = 0; i < numbers.Length; i++)
    {
        for (int j = 0; j < numbers.Length; j++)
        {
            if (i == j)
                continue;
            var pair = new Node(ReadNumber(numbers[i].GetEnumerator()), ReadNumber(numbers[j].GetEnumerator()));
            Reduce(pair);
            int magnitude = CalculateMagnitude(pair);
            if (magnitude > maxMagnitude)
                maxMagnitude = magnitude;
        }
    }
    Console.WriteLine($"Max magnitude is {maxMagnitude}");
}

static Node ReadNumber(CharEnumerator numberRep)
{
    numberRep.MoveNext();
    switch (numberRep.Current)
    {
        case '[':
            var left = ReadNumber(numberRep);
            numberRep.MoveNext();
            Debug.Assert(numberRep.Current == ',');
            var right = ReadNumber(numberRep);
            numberRep.MoveNext();
            Debug.Assert(numberRep.Current == ']');
            return new Node(left, right);
        case char digit and >= '0' and <= '9':
            return new Node(digit - '0');
        default: throw new Exception();
    }
}

static Node Reduce(Node snailfishNumber)
{
    while (DoReduce(snailfishNumber))
    {
    }
    return snailfishNumber;
}

static bool DoReduce(Node snailfishNumber)
{
    return Exploded(snailfishNumber) || Split(snailfishNumber);
}

static bool Exploded(Node snailfishNumber)
{
    bool exploded = false;
    Node? leftmostRegular = null;
    int valueToAddRight = 0;
    foreach (var node in Enumerate(snailfishNumber, 1))
    {
        if (!exploded && node.node.IsReqularNumber)
            leftmostRegular = node.node;
        if (exploded && node.node.IsReqularNumber)
        {
            node.node.Value += valueToAddRight;
            break;
        }

        if (!exploded &&
            node.level > 4 && node.node.IsPair &&
            node.node.Left.IsReqularNumber &&
            node.node.Right.IsReqularNumber)
        {
            exploded = true;
            if (leftmostRegular != null)
                leftmostRegular.Value += node.node.Left.Value;
            valueToAddRight = node.node.Right.Value;
            node.node.ConvertToRegularNumber();
        }
    }
    return exploded;
}


static bool Split(Node snailfishNumber)
{
    bool split = false;
    foreach ((int level, Node node) node in Enumerate(snailfishNumber, 1))
    {
        if (node.node.IsReqularNumber && node.node.Value > 9)
        {
            node.node.Split();
            split = true;
            break;
        }
    }
    return split;
}

static int CalculateMagnitude(Node snailfishNumber) =>
    snailfishNumber.Type switch
    {
        NodeType.RegularNumber => snailfishNumber.Value,
        NodeType.Pair => CalculateMagnitude(snailfishNumber.Left) * 3 + 
                         CalculateMagnitude(snailfishNumber.Right) * 2,
        _ => throw new Exception()
    };

static IEnumerable<(int level, Node node)> Enumerate(Node node, int level)
{
    yield return (level, node);
    if (node.Type == NodeType.RegularNumber)
    {
        yield break;
    }

    foreach (var leftSubtree in Enumerate(node.Left, level + 1))
        yield return leftSubtree;
    foreach (var rightSubtree in Enumerate(node.Right, level + 1))
        yield return rightSubtree;
}