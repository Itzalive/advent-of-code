namespace AdventOfCode2023._2023._14;

public class Day14 : IDay
{
    public int Year => 2023;
    public int Day => 14;

    public string? Part1TestSolution => "136";

    public Task<string> Part1(string input)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        map = TiltNorth(map);
        var score = ScoreMap(map);
        return Task.FromResult(score.ToString());
    }

    private int ScoreMap(char[][] map)
    {
        int score = 0;
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                //Console.Write(map[y][x]);
                if (map[y][x] == 'O')
                {
                    score += map.Length - y;
                }
            }
            //Console.WriteLine();
        }

        return score;
    }

    private char[][] TiltNorth(char[][] map)
    {
        var newMap = new char[map.Length][];
        for (var y = 0; y < map.Length; y++)
        {
            newMap[y] = new char[map[0].Length];
        }

        for (var x = 0; x < map[0].Length; x++)
        {
            var lastUnfilledSpace = 0;
            for (var y = 0; y < map.Length; y++)
            {
                newMap[y][x] = '.';
                if (map[y][x] == 'O')
                {
                    newMap[lastUnfilledSpace][x] = 'O';
                    lastUnfilledSpace++;
                }
                else if (map[y][x] == '#')
                {
                    newMap[y][x] = '#';
                    lastUnfilledSpace = y + 1;
                }
            }
        }

        return newMap;
    }


    private char[][] TiltEast(char[][] map)
    {
        var newMap = new char[map.Length][];
        for (var y = 0; y < map.Length; y++)
        {
            newMap[y] = new char[map[0].Length];
        }

        for (var y = 0; y < map[0].Length; y++)
        {
            var lastUnfilledSpace = 0;
            for (var x = 0; x < map.Length; x++)
            {
                newMap[y][x] = '.';
                if (map[y][x] == 'O')
                {
                    newMap[y][lastUnfilledSpace] = 'O';
                    lastUnfilledSpace++;
                }
                else if (map[y][x] == '#')
                {
                    newMap[y][x] = '#';
                    lastUnfilledSpace = x + 1;
                }
            }
        }

        return newMap;
    }


    private char[][] TiltSouth(char[][] map)
    {
        var newMap = new char[map.Length][];
        for (var y = 0; y < map.Length; y++)
        {
            newMap[y] = new char[map[0].Length];
        }

        for (var x = 0; x < map[0].Length; x++)
        {
            var lastUnfilledSpace = map.Length - 1;
            for (var y = map.Length - 1; y >= 0; y--)
            {
                newMap[y][x] = '.';
                if (map[y][x] == 'O')
                {
                    newMap[lastUnfilledSpace][x] = 'O';
                    lastUnfilledSpace--;
                }
                else if (map[y][x] == '#')
                {
                    newMap[y][x] = '#';
                    lastUnfilledSpace = y - 1;
                }
            }
        }

        return newMap;
    }

    private char[][] TiltWest(char[][] map)
    {
        var newMap = new char[map.Length][];
        for (var y = 0; y < map.Length; y++)
        {
            newMap[y] = new char[map[0].Length];
        }

        for (var y = 0; y < map.Length; y++)
        {
            var lastUnfilledSpace = map[0].Length - 1;
            for (var x = map[0].Length - 1; x >= 0; x--)
            {
                newMap[y][x] = '.';
                if (map[y][x] == 'O')
                {
                    newMap[y][lastUnfilledSpace] = 'O';
                    lastUnfilledSpace--;
                }
                else if (map[y][x] == '#')
                {
                    newMap[y][x] = '#';
                    lastUnfilledSpace = x - 1;
                }
            }
        }

        return newMap;
    }

    public string? Part2TestSolution => "64";


    public Task<string> Part2(string input)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var mapsSeen = new Dictionary<int, List<(char[][], int)>>();
        var hasCycledUp = false;
        for (var i = 0; i < 1000000000; i++)
        {
            map = TiltNorth(map);
            map = TiltEast(map);
            map = TiltSouth(map);
            map = TiltWest(map);

            if (hasCycledUp) continue;

            var scoreMap = ScoreMap(map);
            if (mapsSeen.TryGetValue(scoreMap, out var existingMaps))
            {
                foreach (var existingMap in existingMaps)
                {
                    var isMatch = true;
                    for (var y = 0; y < map.Length && isMatch; y++)
                    {
                        for (var x = 0; x < map[0].Length && isMatch; x++)
                        {
                            if (map[y][x] != existingMap.Item1[y][x])
                            {
                                isMatch = false;
                            }
                        }
                    }

                    if (isMatch)
                    {
                        var cycleTime = i - existingMap.Item2;
                        Console.WriteLine($"Found this map {cycleTime} cycles ago, after a total of {i + 1} cycles");
                        while (i < 1000000000 - cycleTime)
                        {
                            i += cycleTime;
                        }

                        hasCycledUp = true;
                    }
                }
            }
            else
            {
                mapsSeen.Add(scoreMap, [(map, i)]);
            }
        }

        var score = ScoreMap(map);
        return Task.FromResult(score.ToString());
    }

    public string? TestInput => @"O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....";
}