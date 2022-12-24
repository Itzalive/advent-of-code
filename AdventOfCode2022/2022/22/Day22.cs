using System.Text.RegularExpressions;

namespace AdventOfCode2022._2022._22;

public class Day22 : IDay
{
    public int Year => 2022;

    public int Day => 22;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var map = inputs[0].Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var commands = Regex.Split(inputs[1], $@"(R|L)")
            .Where(p => p != string.Empty)
            .ToList();

        var currentY = 0;
        var currentX = Array.FindIndex(map[0], e => e == '.');
        var direction = 0;
        foreach (var command in commands)
        {
            if (int.TryParse(command, out var distance))
            {
                for (var i = 0; i < distance; i++)
                {
                    var nextX = currentX;
                    var nextY = currentY;
                    do
                    {
                        switch (direction)
                        {
                            case 0:
                                nextX = (nextX + 1) % map[nextY].Length;
                                break;
                            case 1:
                                nextY = (nextY + 1) % map.Length;
                                break;
                            case 2:
                                nextX = (nextX - 1 + map[nextY].Length) % map[nextY].Length;
                                break;
                            case 3:
                                nextY = (nextY - 1  + map.Length) % map.Length;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    } while (map[nextY].Length < nextX || map[nextY][nextX] == ' ');

                    if (map[nextY][nextX] != '#')
                    {
                        map[currentY][currentX] =
                            direction == 0 ? '>' : direction == 1 ? 'v' : direction == 2 ? '<' : '^';
                        currentX = nextX;
                        currentY = nextY;
                    }
                    else if (map[nextY][nextX] == '#')
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine(map[nextY][nextX]);
                        throw new NotImplementedException();
                    }
                }
            } else
                direction = command switch
                {
                    "L" => (direction + 3) % 4,
                    "R" => (direction + 1) % 4,
                    _ => throw new NotImplementedException()
                };
            
            map[currentY][currentX] =
                direction == 0 ? '>' : direction == 1 ? 'v' : direction == 2 ? '<' : '^';

            //Console.WriteLine($"{currentX}, {currentY}");
            //foreach (var l in map)
            //    Console.WriteLine(string.Join("", l));
        }

        return Task.FromResult(((currentY+1) * 1000 + (currentX+1) * 4 + direction).ToString());
    }

    
    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var map = inputs[0].Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var commands = Regex.Split(inputs[1], $@"(R|L)")
            .Where(p => p != string.Empty)
            .ToList();

        var currentY = 0;
        var currentX = Array.FindIndex(map[0], e => e == '.');
        var direction = 0;
        var cubeSize = map.Length > 20 ? map.Length / 4 : map.Length / 3;
        foreach (var command in commands)
        {
            try
            {
                if (int.TryParse(command, out var distance))
                {
                    for (var i = 0; i < distance; i++)
                    {
                        var nextX = currentX;
                        var nextY = currentY;
                        var nextDirection = direction;
                        if (cubeSize > 10)
                        {
                            switch (direction)
                            {
                                case 0:
                                    nextX = nextX + 1;
                                    if (nextX >= map[nextY].Length)
                                    {
                                        switch ((int) (nextY / cubeSize))
                                        {
                                            case 0:
                                                nextY = cubeSize * 3 - 1 - DistanceAlongCubeEdge(0, nextY, cubeSize);
                                                nextX = 2 * cubeSize - 1;
                                                nextDirection = 2;
                                                break;
                                            case 1:
                                                nextX = 2 * cubeSize + DistanceAlongCubeEdge(1, nextY, cubeSize);
                                                nextY = cubeSize - 1;
                                                nextDirection = 3;
                                                break;
                                            case 2:
                                                nextY = cubeSize - 1 - DistanceAlongCubeEdge(2, nextY, cubeSize);
                                                nextX = cubeSize * 3 - 1;
                                                nextDirection = 2;
                                                break;
                                            case 3:
                                                nextX = cubeSize + DistanceAlongCubeEdge(3, nextY, cubeSize);
                                                nextY = 3 * cubeSize - 1;
                                                nextDirection = 3;
                                                break;
                                        }
                                    }

                                    break;
                                case 1:
                                    nextY = nextY + 1;
                                    if (nextY >= map.Length || map[nextY].Length <= nextX || map[nextY][nextX] == ' ')
                                    {
                                        switch ((int) (nextX / cubeSize))
                                        {
                                            case 0:
                                                nextX = 2 * cubeSize + DistanceAlongCubeEdge(0, nextX, cubeSize);
                                                nextY = 0;
                                                nextDirection = 1;
                                                break;
                                            case 1:
                                                nextY = 3 * cubeSize + DistanceAlongCubeEdge(1, nextX, cubeSize);
                                                nextX = cubeSize - 1;
                                                nextDirection = 2;
                                                break;
                                            case 2:
                                                nextY = cubeSize + DistanceAlongCubeEdge(2, nextX, cubeSize);
                                                nextX = 2 * cubeSize - 1;
                                                nextDirection = 2;
                                                break;
                                        }
                                    }

                                    break;
                                case 2:
                                    nextX = nextX - 1;
                                    if (nextX < 0 || map[nextY][nextX] == ' ')
                                    {
                                        switch ((int) (nextY / cubeSize))
                                        {
                                            case 0:
                                                nextY = 3 * cubeSize - 1 - DistanceAlongCubeEdge(0, nextY, cubeSize);
                                                nextX = 0;
                                                nextDirection = 0;
                                                break;
                                            case 1:
                                                nextX = DistanceAlongCubeEdge(1, nextY, cubeSize);
                                                nextY = cubeSize * 2;
                                                nextDirection = 1;
                                                break;
                                            case 2:
                                                nextY = cubeSize - 1 - DistanceAlongCubeEdge(2, nextY, cubeSize);
                                                nextX = cubeSize;
                                                nextDirection = 0;
                                                break;
                                            case 3:
                                                nextX = cubeSize + DistanceAlongCubeEdge(3, nextY, cubeSize);
                                                nextY = 0;
                                                nextDirection = 1;
                                                break;
                                        }
                                    }

                                    break;
                                case 3:
                                    nextY = nextY - 1;
                                    if (nextY < 0 || map[nextY].Length <= nextX || map[nextY][nextX] == ' ')
                                    {
                                        switch ((int) (nextX / cubeSize))
                                        {
                                            case 0:
                                                nextY = cubeSize + nextX;
                                                nextX = cubeSize;
                                                nextDirection = 0;
                                                break;
                                            case 1:
                                                nextY = 3 * cubeSize + DistanceAlongCubeEdge(1, nextX, cubeSize);
                                                nextX = 0;
                                                nextDirection = 0;
                                                break;
                                            case 2:
                                                nextX = DistanceAlongCubeEdge(2, nextX, cubeSize);
                                                nextY = 4 * cubeSize - 1;
                                                break;
                                        }
                                    }

                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }
                        else
                        {
                            switch (direction)
                            {
                                case 0:
                                    nextX = nextX + 1;
                                    if (nextX >= map[nextY].Length)
                                    {
                                        switch ((int) (nextY / cubeSize))
                                        {
                                            case 0:
                                            case 2:
                                                nextY = map.Length - 1 - nextY;
                                                nextX = map[nextY].Length - 1;
                                                nextDirection = (direction + 2) % 4;
                                                break;
                                            case 1:
                                                nextX = map[cubeSize * 2].Length - ((nextY - 1) % cubeSize) - 2;
                                                nextY = cubeSize * 2;
                                                nextDirection = (direction + 1) % 4;
                                                break;
                                        }
                                    }

                                    break;
                                case 1:
                                    nextY = nextY + 1;
                                    if (nextY >= map.Length || map[nextY][nextX] == ' ')
                                    {
                                        switch ((int) (nextX / cubeSize))
                                        {
                                            case 0:
                                                nextX = 3 * cubeSize - 1 - nextX;
                                                nextY = 3 * cubeSize - 1;
                                                nextDirection = (direction + 2) % 4;
                                                break;
                                            case 1:
                                                nextY = 4 * cubeSize - nextX;
                                                nextX = 2 * cubeSize;
                                                break;
                                            case 2:
                                                nextX = 3 * cubeSize - 1 - nextX;
                                                nextY = 2 * cubeSize - 1;
                                                nextDirection = (direction + 2) % 4;
                                                break;
                                            case 3:
                                                nextY = cubeSize + (map[nextY].Length - 1 - nextX);
                                                nextX = 0;
                                                nextDirection = (direction + 3) % 4;
                                                break;
                                        }
                                    }

                                    break;
                                case 2:
                                    nextX = nextX - 1;
                                    if (nextX < 0 || map[nextY][nextX] == ' ')
                                    {
                                        switch ((int) (nextY / cubeSize))
                                        {
                                            case 0:
                                                nextX = cubeSize + nextY;
                                                nextY = cubeSize;
                                                nextDirection = (direction + 3) % 4;
                                                break;
                                            case 1:
                                                nextX = cubeSize * 3 - 1 + nextY;
                                                nextY = cubeSize * 3 - 1;
                                                nextDirection = (direction + 1) % 4;
                                                break;
                                            case 2:
                                                nextX = 4 * cubeSize - nextY - 1;
                                                nextY = 2 * cubeSize - 1;
                                                nextDirection = (direction + 1) % 4;
                                                break;
                                        }
                                    }

                                    break;
                                case 3:
                                    nextY = nextY - 1;
                                    if (nextY < 0 || map[nextY].Length <= nextX || map[nextY][nextX] == ' ')
                                    {
                                        switch ((int) (nextX / cubeSize))
                                        {
                                            case 0:
                                                nextX = 3 * cubeSize - 1 - nextX;
                                                nextY = 0;
                                                nextDirection = (direction + 2) % 4;
                                                break;
                                            case 1:
                                                nextY = nextX - cubeSize;
                                                nextX = 2 * cubeSize;
                                                break;
                                            case 2:
                                                nextX = 3 * cubeSize - 1 - nextX;
                                                nextY = cubeSize;
                                                nextDirection = (direction + 2) % 4;
                                                break;
                                            case 3:
                                                nextY = cubeSize + (map[nextY].Length - 1 - nextX);
                                                nextX = 3 * cubeSize - 1;
                                                nextDirection = (direction + 3) % 4;
                                                break;
                                        }
                                    }

                                    break;
                                default:
                                    throw new NotImplementedException();
                            }
                        }

                        if (map[nextY][nextX] != '#')
                        {
                            map[currentY][currentX] =
                                direction == 0 ? '>' : direction == 1 ? 'v' : direction == 2 ? '<' : '^';
                            currentX = nextX;
                            currentY = nextY;
                            direction = nextDirection;
                        }
                        else if (map[nextY][nextX] == '#')
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine(map[nextY][nextX]);
                            throw new NotImplementedException();
                        }
                    }
                }
                else
                    direction = command switch
                    {
                        "L" => (direction + 3) % 4,
                        "R" => (direction + 1) % 4,
                        _ => throw new NotImplementedException()
                    };

                map[currentY][currentX] =
                    direction == 0 ? '>' : direction == 1 ? 'v' : direction == 2 ? '<' : '^';

                //if (map.Length < 20)
                {
                    Console.WriteLine($"{currentX}, {currentY}");
                    foreach (var l in map)
                        Console.WriteLine(string.Join("", l));
                }
            }
            catch
            {
                Console.WriteLine($"Direction: {direction}");
                Console.WriteLine(currentX + ", " + currentY);
                throw;
            }
        }

        return Task.FromResult(((currentY+1) * 1000 + (currentX+1) * 4 + direction).ToString());
    }

    private int DistanceAlongCubeEdge(int cubes, int distance, int cubeSize)
    {
        return distance - cubes * cubeSize;
    }

    public string? TestInput => @"        ...#
        .#..
        #...
        ....
...#.......#
........#...
..#....#....
..........#.
        ...#....
        .....#..
        .#......
        ......#.

10R5L5R10L4R5L5";
}