using System.Collections;
using AdventOfCode2023._2023._21;

namespace AdventOfCode2023._2024._16;

public class Day16 : IDay
{
    public int Year => 2024;

    public int Day => 16;

    public string? Part1TestSolution => "11048";

    public Task<string> Part1(string input)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var mapScore = Enumerable.Range(0, map.Length)
            .Select(_ => Enumerable.Range(0, map[0].Length).Select(_ => new[] { int.MaxValue, int.MaxValue, int.MaxValue, int.MaxValue }).ToArray()).ToArray();
        var posY = -1;
        var posX = -1;
        var endY = -1;
        var endX = -1;
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == 'S')
                {
                    posY = y;
                    posX = x;
                }

                if (map[y][x] == 'E')
                {
                    endY = y;
                    endX = x;
                }
            }
        }

        var directions = new[] { (X: 1, Y: 0), (X: 0, Y: -1), (X: -1, Y: 0), (X: 0, Y: 1) };
        var toCheck = new Queue<(int X, int Y, int Direction, int Score)>();
        toCheck.Enqueue((posX, posY, 0, 0));
        while (toCheck.Any())
        {
            var checking = toCheck.Dequeue();
            for (var i = 0; i < 4; i++)
            {
                var checkDirection = (checking.Direction + i) % 4;
                var direction = directions[checkDirection];
                var newScore = checking.Score + (i == 3 ? 1000: i * 1000) + 1;
                if (map[checking.Y + direction.Y][checking.X + direction.X] != '#' && mapScore[checking.Y + direction.Y][checking.X + direction.X][checkDirection] > newScore)
                {
                    mapScore[checking.Y + direction.Y][checking.X + direction.X][checkDirection] = newScore;
                    toCheck.Enqueue((checking.X + direction.X, checking.Y + direction.Y, checkDirection, newScore));
                }
            }
        }

        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '#')
                {
                    Console.Write('#');
                }
                else
                {
                    var minScore = mapScore[y][x].Min();
                    if (minScore == int.MaxValue)
                    {
                        Console.Write('.');
                    }
                    else
                    {
                        if (mapScore[y][x][0] == minScore)
                        {
                            Console.Write('>');
                        }
                        else if (mapScore[y][x][1] == minScore)
                        {
                            Console.Write('^');
                        }
                        else if (mapScore[y][x][2] == minScore)
                        {
                            Console.Write('<');
                        }
                        else if (mapScore[y][x][3] == minScore)
                        {
                            Console.Write('v');
                        }
                    }
                }
            }
            Console.WriteLine();
        }

        return Task.FromResult(mapScore[endY][endX].Min().ToString());
    }

    public string? Part2TestSolution => "64";


    public Task<string> Part2(string input)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var mapScore = Enumerable.Range(0, map.Length)
            .Select(_ => Enumerable.Range(0, map[0].Length).Select(_ => new[]
            {
                (Score: int.MaxValue, Pathway: new List<(int X, int Y)>()),
                (Score: int.MaxValue, Pathway: new List<(int X, int Y)>()),
                (Score: int.MaxValue, Pathway: new List<(int X, int Y)>()),
                (Score: int.MaxValue, Pathway: new List<(int X, int Y)>())
            }).ToArray()).ToArray();
        var posY = -1;
        var posX = -1;
        var endY = -1;
        var endX = -1;
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == 'S')
                {
                    posY = y;
                    posX = x;
                }

                if (map[y][x] == 'E')
                {
                    endY = y;
                    endX = x;
                }
            }
        }

        var directions = new[] { (X: 1, Y: 0), (X: 0, Y: -1), (X: -1, Y: 0), (X: 0, Y: 1) };
        var toCheck = new Queue<(int X, int Y, int Direction, int Score, List<(int X, int Y)> Pathway)>();
        toCheck.Enqueue((posX, posY, 0, 0, []));
        while (toCheck.Any())
        {
            var checking = toCheck.Dequeue();
            for (var i = 0; i < 4; i++)
            {
                var checkDirection = (checking.Direction + i) % 4;
                var direction = directions[checkDirection];
                var newScore = checking.Score + (i == 3 ? 1000 : i * 1000) + 1;
                if (map[checking.Y + direction.Y][checking.X + direction.X] != '#' &&
                    mapScore[checking.Y + direction.Y][checking.X + direction.X][checkDirection].Score >= newScore)
                {
                    if (mapScore[checking.Y + direction.Y][checking.X + direction.X][checkDirection].Score == newScore)
                    {
                        mapScore[checking.Y + direction.Y][checking.X + direction.X][checkDirection].Pathway =
                            [
                                .. mapScore[checking.Y + direction.Y][checking.X + direction.X][checkDirection].Pathway,
                                ..checking.Pathway, (checking.X, checking.Y)
                            ];
                        mapScore[checking.Y + direction.Y][checking.X + direction.X][checkDirection].Pathway =
                            mapScore[checking.Y + direction.Y][checking.X + direction.X][checkDirection].Pathway
                                .Distinct()
                                .ToList();
                    }
                    else
                    {
                        mapScore[checking.Y + direction.Y][checking.X + direction.X][checkDirection].Pathway = [..checking.Pathway, (checking.X, checking.Y)];
                    }

                    mapScore[checking.Y + direction.Y][checking.X + direction.X][checkDirection].Score = newScore;
                    toCheck.Enqueue((checking.X + direction.X, checking.Y + direction.Y, checkDirection, newScore,
                        mapScore[checking.Y + direction.Y][checking.X + direction.X][checkDirection].Pathway));
                }
            }
        }


        var minScore = mapScore[endY][endX].MinBy(x => x.Score).Score;
        var bestPathways = mapScore[endY][endX].Where(c => c.Score == minScore).SelectMany(c => c.Pathway).Distinct()
            .ToArray();
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                if (map[y][x] == '#')
                {
                    Console.Write('#');
                }
                else if (bestPathways.Any(p => p.X == x && p.Y == y))
                {
                    Console.Write('0');
                }
                else
                {
                    Console.Write('.');
                }
            }
            Console.WriteLine();
        }

        return Task.FromResult((bestPathways.Length + 1).ToString());
    }

    public string? TestInput => @"#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################";
}