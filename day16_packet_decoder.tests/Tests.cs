using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;

namespace day16_packet_decoder;

[TestFixture]
public class Tests
{
    [TestCase("D2FE28", "110100101111111000101000")]
    [TestCase("38006F45291200", "00111000000000000110111101000101001010010001001000000000")]
    public void HexToBinaryStringTest(string source, string expected)
    {
        string converted = Program.ConvertHexToBitString(source);
        Assert.That(converted, Is.EqualTo(expected));
    }
    
    [TestCase("D2FE28", 6)]
    [TestCase("38006F45291200", 9)]
    [TestCase("8A004A801A8002F478", 16)]
    [TestCase("620080001611562C8802118E34", 12)]
    [TestCase("C0015000016115A2E0802F182340", 23)]
    [TestCase("A0016C880162017C3686B18A3D4780", 31)]
    public void Part1SumOfVersionsTest(string input, int expectedSum)
    {
        Assert.That(Program.Solve(input).sumOfVersions, Is.EqualTo(expectedSum));
    }

    [TestCase("C200B40A82", 3)] // the sum of 1 and 2, resulting in the value 3.
    [TestCase("04005AC33890", 54)] // the product of 6 and 9, resulting in the value 54.
    [TestCase("880086C3E88112", 7)] // the minimum of 7, 8, and 9, resulting in the value 7.
    [TestCase("CE00C43D881120", 9)] // the maximum of 7, 8, and 9, resulting in the value 9.
    [TestCase("D8005AC2A8F0", 1)] // produces 1, because 5 is less than 15.
    [TestCase("F600BC2D8F", 0)] // produces 0, because 5 is not greater than 15.
    [TestCase("9C005AC2F8F0", 0)] // produces 0, because 5 is not equal to 15.
    [TestCase("9C0141080250320F1802104A08", 1)] // produces 1, because 1 + 3 = 2 * 2.
    public void Part2ResultTest(string input, int expected)
    {
        Assert.That(Program.Solve(input).result, Is.EqualTo(expected));
    }
}