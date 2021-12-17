using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("day16_packet_decoder.tests")]

namespace day16_packet_decoder;

public class Program
{
    public static void Main()
    {
        Solve(ReadInput("input.txt"));
    }


    enum ParseState
    {
        Version,
        Type,
        Literal,
        Operator,
    }

    enum LengthType
    {
        Bits,
        NumberOfSubPackets
    }

    enum PacketType
    {
        Sum,
        Product,
        Min,
        Max,
        Literal,
        GreaterThan,
        LessThan,
        Equal
    }

    class ParentPacket
    {
        public PacketType OperatorType { get; }
        public LengthType LengthType { get; }
        public int NumberOfPacketsRemaining { get; set; }
        public int BitLength { get; }
        public int StartReadPos { get; }
        public int EndReadPos => StartReadPos + BitLength;
        public Stack<long> Values { get; }

        private ParentPacket(PacketType operatorType, LengthType lengthType)
        {
            OperatorType = operatorType;
            LengthType = lengthType;
            Values = new Stack<long>();
        }

        public ParentPacket(PacketType operatorType, LengthType lengthType, int numberOfPacketsRemaining)
            : this(operatorType, lengthType)
        {
            NumberOfPacketsRemaining = numberOfPacketsRemaining;
        }

        public ParentPacket(PacketType operatorType, LengthType lengthType, int startReadPos, int bitLength)
            : this(operatorType, lengthType)
        {
            StartReadPos = startReadPos;
            BitLength = bitLength;
        }
    }

    private static readonly Dictionary<char, string> HexMap = new()
    {
        {'0', "0000"},
        {'1', "0001"},
        {'2', "0010"},
        {'3', "0011"},
        {'4', "0100"},
        {'5', "0101"},
        {'6', "0110"},
        {'7', "0111"},
        {'8', "1000"},
        {'9', "1001"},
        {'A', "1010"},
        {'B', "1011"},
        {'C', "1100"},
        {'D', "1101"},
        {'E', "1110"},
        {'F', "1111"},
    };

    internal static (int sumOfVersions, long result) Solve(string input)
    {
        string bitString = ConvertHexToBitString(input);
        // Console.WriteLine(bitString);
        string binaryResultingString = "";
        var state = ParseState.Version;
        var parentPackets = new Stack<ParentPacket>();
        using var iter = new MyCharEnumerator(bitString);
        iter.MoveNext();
        int sumOfVersions = 0;
        int version = 0;
        string literal = "";
        long result = 0;
        PacketType type = 0;
        do
        {
            switch (state)
            {
                case ParseState.Version:
                    version = ReadVersion(iter);
                    sumOfVersions += version;
                    state = ParseState.Type;
                    break;
                case ParseState.Type:
                    type = (PacketType) ReadType(iter);
                    state = type == PacketType.Literal ? ParseState.Literal : ParseState.Operator;
                    break;
                case ParseState.Literal:
                    bool hasMore = iter.Current == '1';
                    iter.MoveNext();
                    literal += ReadBits(iter, 4);
                    if (!hasMore)
                    {
                        // long longValue = Convert.ToInt64(literal, 2);
                        // int value = (int) longValue;
                        long value = Convert.ToInt64(literal, 2);
                        while (parentPackets.TryPeek(out var parentPacket))
                        {
                            parentPacket.Values.Push(value);
                            if (parentPacket.LengthType == LengthType.NumberOfSubPackets &&
                                --parentPacket.NumberOfPacketsRemaining <= 0 ||
                                parentPacket.LengthType == LengthType.Bits &&
                                iter.Index + 1 >= parentPacket.EndReadPos)
                            {
                                var parent = parentPackets.Pop();
                                value = CalcResult(parent);
                                continue;
                            }
                            break;
                        }

                        if (parentPackets.Count == 0)
                        {
                            result = value;
                            goto end;
                        }

                        state = ParseState.Version;
                        literal = "";
                    }
                    break;
                case ParseState.Operator:
                    var lenType = ReadBits(iter, 1) == "0" ? LengthType.Bits : LengthType.NumberOfSubPackets;
                    iter.MoveNext();
                    if (lenType == LengthType.NumberOfSubPackets)
                    {
                        int numOfSubPackets = Convert.ToInt32(ReadBits(iter, 11), 2);
                        parentPackets.Push(new ParentPacket(type, lenType, numOfSubPackets));
                    }
                    else
                    {
                        int packetBitsLen = Convert.ToInt32(ReadBits(iter, 15), 2);
                        int startReadPos = iter.Index + 1;
                        parentPackets.Push(new ParentPacket(type, lenType, startReadPos, packetBitsLen));
                    }
                    state = ParseState.Version;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported state {state}");
            }
        } while (iter.MoveNext());
        
        end:
        Console.WriteLine(binaryResultingString);
        Console.WriteLine($"Sum of packet's versions: {sumOfVersions}");
        Console.WriteLine($"Result of BITS expression: {result}");
        return (sumOfVersions, result);
    }

    internal static string ConvertHexToBitString(string input)
    {
        var bitStringBuilder = new StringBuilder(input.Length * 4);
        foreach (char c in input)
            bitStringBuilder.Append(HexMap[c]);
        return bitStringBuilder.ToString();
    }

    static string ReadInput(string filename) => File.ReadAllLines(filename)[0];

    static int ReadVersion(MyCharEnumerator iter)
    {
        string version = ReadBits(iter, 3);
        return Convert.ToInt32(version, 2);
    }

    static int ReadType(MyCharEnumerator iter) => ReadVersion(iter);

    static string ReadBits(MyCharEnumerator charEnumerator, int len)
    {
        var chars = new char[len];
        chars[0] = charEnumerator.Current;
        for (int i = 1; i < len; i++)
        {
            charEnumerator.MoveNext();
            chars[i] = charEnumerator.Current;
        }
        return new string(chars);
    }

    static long CalcResult(ParentPacket packet)
    {
        var values = GetValues(packet.Values);
        switch (packet.OperatorType)
        {
            case PacketType.Sum: return values.Sum();
            case PacketType.Product: return values.Aggregate(1l, (aggr, v) => aggr * v);
            case PacketType.Min: return values.Min();
            case PacketType.Max: return values.Max();
            case PacketType.GreaterThan:
            case PacketType.LessThan:
            case PacketType.Equal:
                long[] vals = values.ToArray();
                Debug.Assert(vals.Length == 2);
                bool result = packet.OperatorType switch
                {
                    PacketType.GreaterThan => vals[0] < vals[1], // values from stack came reversed
                    PacketType.LessThan => vals[0] > vals[1],
                    _ => vals[0] == vals[1]
                };
                return result ? 1 : 0;
            default:
                throw new InvalidOperationException($"Unsupported operator type {packet.OperatorType}");
        }
    }

    static IEnumerable<long> GetValues(Stack<long> stack)
    {
        while (stack.TryPop(out long value))
            yield return value;
    }
}