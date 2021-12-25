// See https://aka.ms/new-console-template for more information

using utils;

// Part1(@"
// v...>>.vv>
// .vv>>.vv..
// >>.>v>...v
// >>v>>.>.v.
// v>v.vv.v..
// >.>>..v...
// .vv..>.>v.
// v.v..>>v.v
// ....v..v.>");

using (new MyStopwatch())
    Part1(File.ReadAllText("input.txt"));


static void Part1(string input)
{
    var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    int DimY = lines.Length;
    int DimX = lines[0].Length;
    var map = new char[DimY, DimX];
    for (int i = 0; i < DimY; i++)
    for (int j = 0; j < DimX; j++)
        map[i, j] = lines[i][j];
    // PrintMap(map, DimX, DimY);

    int step = 0;
    while (true)
    {
        bool movedEast = MoveEastGroup(map, DimX, DimY);
        bool movedSouth = MoveSouthGroup(map, DimX, DimY);
        if (!movedEast && !movedSouth)
            break;
        step++;
        // Console.WriteLine($"After {step} steps:");
        // PrintMap(map, DimX, DimY);
        // Console.WriteLine();
    }
    Console.WriteLine($"First step on which no sea cucumbers move: {step + 1}");
    // PrintMap(map, DimX, DimY);
}

static bool MoveEastGroup(char[,] map, int dimX, int dimY)
{
    var pointsToMove = new HashSet<Point>(50);
    for (int i = 0; i < dimY; i++) // go horizontal
    for (int j = 0; j < dimX; j++)
    {
        if (map[i, j] == '>' &&
            map[i, (j + 1) % dimX] == '.')
        {
            pointsToMove.Add(new Point(i, j));
        }
    }

    foreach (var point in pointsToMove)
    {
        int i = point.x;
        int j = point.y;
        map[i, j] = '.';
        map[i, (j + 1) % dimX] = '>';
    }

    return pointsToMove.Count > 0;
}

static bool MoveSouthGroup(char[,] map, int dimX, int dimY)
{
    var pointsToMove = new HashSet<Point>(50);
    for (int j = 0; j < dimX; j++) // go vertical
    for (int i = 0; i < dimY; i++)
    {
        if (map[i, j] == 'v' &&
            map[(i + 1) % dimY, j] == '.')
        {
            pointsToMove.Add(new Point(i, j));
        }
    }

    foreach (var point in pointsToMove)
    {
        int i = point.x;
        int j = point.y;
        map[i, j] = '.';
        map[(i + 1) % dimY, j] = 'v';
    }

    return pointsToMove.Count > 0;
}

static void PrintMap(char[,] map, int dimX, int dimY)
{
    for (int i = 0; i < dimY; i++) // go horizontal
    {
        for (int j = 0; j < dimX; j++)
        {
            Console.Write(map[i, j]);
        }
        Console.WriteLine();
    }
}

record Point(int x, int y);