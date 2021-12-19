// See https://aka.ms/new-console-template for more information

using System.Collections.Concurrent;

// test input
// Part1((20, 30), (-10, -5));
// actual input
Part1((241, 273), (-97, -63));

static void Part1((int x1, int x2) rangeX, (int y1, int y2) rangeY)
{
    int xMin = int.MaxValue;
    int xMax = 0;
    for (int startX = 0; startX <= rangeX.x2; startX++)
    {
        int x = 0;
        int xVel = startX;
        while (xVel > 0)
        {
            x += xVel--;
            if (x >= rangeX.x1 && x <= rangeX.x2)
            {
                if (startX < xMin)
                    xMin = startX;
                if (startX > xMax)
                    xMax = startX;
                break;
            }
            if (x > rangeX.x2)
                break;
        }
    }
    Console.WriteLine($"X velocity range: {xMin} to {xMax}");

    int highestY = 0;
    object highestYlock = new();
    var velocities = new ConcurrentDictionary<(int x, int y), int>(concurrencyLevel: 24, capacity: 1000);
    Parallel.ForEach(Enumerable.Range(xMin, xMax - xMin + 1), startX =>
    {
        int highestYtemp = 0;
        for (int startY = rangeY.y1; startY < 1000; startY++)
        {
            int xVel = startX;
            int yVel = startY;
            int x = xVel;
            int y = yVel;
            int curHighestY = 0;
            bool hit = false;
            for (int step = 1; step < 1000; step++)
            {
                if (yVel == 0) curHighestY = y;
                if (x >= rangeX.x1 && x <= rangeX.x2 &
                    y >= rangeY.y1 && y <= rangeY.y2)
                {
                    hit = true;
                    velocities[(startX, startY)] = 1;
                    break;
                }

                if (y < rangeY.y1) // missed and passed through
                    break;

                if (xVel > 0) xVel--;
                yVel--;
                x += xVel;
                y += yVel;
            }

            if (hit && highestYtemp < curHighestY)
                highestYtemp = curHighestY;
        }

        lock (highestYlock)
            if (highestY < highestYtemp)
                highestY = highestYtemp;

    });

    Console.WriteLine($"Highest y is {highestY}");
    Console.WriteLine($"Distinct velocities: {velocities.Count}");
}