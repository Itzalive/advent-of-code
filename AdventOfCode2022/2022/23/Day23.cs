using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode2022._2022._23;

public class Day23 : IDay
{
    public int Year => 2022;

    public int Day => 23;

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();

        // map a bigger map
        var map = new bool[inputs.Length * 3][];
        var mapNext = new bool[inputs.Length * 3][];
        for (var y = 0; y < map.Length; y++)
        {
            map[y] = new bool[inputs[0].Length * 3];
            mapNext[y] = new bool[inputs[0].Length * 3];
        }

        for (var y = 0; y < inputs.Length; y++)
        {
            for (var x = 0; x < inputs[y].Length; x++)
            {
                map[y + inputs.Length][x + inputs[y].Length] = inputs[y][x] == '#';
            }
        }

        var directions = new List<char>(new[] {'N', 'S', 'W', 'E'});

        for (var round = 0; round < 10; round++)
        {
            var moves = new Dictionary<(int, int), (int, int)>();
            for (var y = 1; y < map.Length - 1; y++)
            {
                for (var x = 1; x < map[y].Length - 1; x++)
                {
                    if (!map[y][x]) continue;
                    var north = map[y - 1][x-1] || map[y - 1][x] || map[y - 1][x + 1];
                    var south = map[y + 1][x-1] || map[y + 1][x] || map[y + 1][x + 1];
                    var west = map[y - 1][x-1] || map[y][x - 1] || map[y + 1][x - 1];
                    var east = map[y - 1][x+1] || map[y][x + 1] || map[y + 1][x + 1];

                    if (north || south || west || east)
                    {
                        for (var i = 0; i < 5; i++)
                        {
                            if (i == 4)
                            {
                                mapNext[y][x] = true;
                                break;
                            }

                            switch (directions[i])
                            {
                                case 'N':
                                    if (!north)
                                    {
                                        moves.Add((x, y), (x, y - 1));
                                        i = 5;
                                    }
                                    break;
                                
                                case 'S':
                                    if (!south)
                                    {
                                        moves.Add((x, y), (x, y + 1));
                                        i = 5;
                                    }
                                    break;
                                
                                case 'W':
                                    if (!west)
                                    {
                                        moves.Add((x, y), (x - 1, y));
                                        i = 5;
                                    }
                                    break;
                                
                                case 'E':
                                    if (!east)
                                    {
                                        moves.Add((x, y), (x + 1, y));
                                        i = 5;
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        mapNext[y][x] = true;
                    }
                }
            }
            directions.Add(directions[0]);
            directions.RemoveAt(0);

            foreach (var group in moves.GroupBy(kvp => kvp.Value))
            {
                if (group.Count() == 1)
                {
                    var pos = group.Single().Value;
                    mapNext[pos.Item2][pos.Item1] = true;
                }
                else
                {
                    foreach (var pos in group.Select(i => i.Key))
                    {
                        mapNext[pos.Item2][pos.Item1] = true;
                    }
                }
            }

            (map, mapNext) = (mapNext, map);
            foreach (var row in mapNext)
                for (var x = 0; x < row.Length; x++)
                    row[x] = false;
            if (map.Length < 30)
            {
                Console.WriteLine();
                Console.WriteLine("Round " + round);
                foreach (var r in map)
                    Console.WriteLine(string.Join("", r.Select(c => c ? '#' : '.')));
            }
        }

        var minX = map.Where(r => Array.IndexOf(r, true) > -1).Min(r => Array.IndexOf(r, true));
        var maxX = map.Max(r => Array.LastIndexOf(r, true));
        var score = 0;
        var afterScore = 0;
        for (var y = 0; y < map.Length; y++)
        {
            if (score == 0 && !map[y].Any(t => t)) continue;
            if (!map[y].Any(t => t))
            {
                afterScore += map[y].Count(t => !t) - map[y].Length + maxX - minX + 1;
            }
            else
            {
                score += afterScore;
                afterScore = 0;
                score += map[y].Count(t => !t) - map[y].Length + maxX - minX + 1;
            }
        }
        return Task.FromResult(score.ToString());
    }

    
    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();

        // map a bigger map
        var map = new bool[inputs.Length * 3][];
        var mapNext = new bool[inputs.Length * 3][];
        for (var y = 0; y < map.Length; y++)
        {
            map[y] = new bool[inputs[0].Length * 3];
            mapNext[y] = new bool[inputs[0].Length * 3];
        }

        for (var y = 0; y < inputs.Length; y++)
        {
            for (var x = 0; x < inputs[y].Length; x++)
            {
                map[y + inputs.Length][x + inputs[y].Length] = inputs[y][x] == '#';
            }
        }

        var directions = new List<char>(new[] {'N', 'S', 'W', 'E'});

        for (var round = 0; true; round++)
        {
            var moves = new Dictionary<(int, int), (int, int)>();
            for (var y = 1; y < map.Length - 1; y++)
            {
                for (var x = 1; x < map[y].Length - 1; x++)
                {
                    if (!map[y][x]) continue;
                    var north = map[y - 1][x-1] || map[y - 1][x] || map[y - 1][x + 1];
                    var south = map[y + 1][x-1] || map[y + 1][x] || map[y + 1][x + 1];
                    var west = map[y - 1][x-1] || map[y][x - 1] || map[y + 1][x - 1];
                    var east = map[y - 1][x+1] || map[y][x + 1] || map[y + 1][x + 1];

                    if (north || south || west || east)
                    {
                        for (var i = 0; i < 5; i++)
                        {
                            if (i == 4)
                            {
                                mapNext[y][x] = true;
                                break;
                            }

                            switch (directions[i])
                            {
                                case 'N':
                                    if (!north)
                                    {
                                        moves.Add((x, y), (x, y - 1));
                                        i = 5;
                                    }
                                    break;
                                
                                case 'S':
                                    if (!south)
                                    {
                                        moves.Add((x, y), (x, y + 1));
                                        i = 5;
                                    }
                                    break;
                                
                                case 'W':
                                    if (!west)
                                    {
                                        moves.Add((x, y), (x - 1, y));
                                        i = 5;
                                    }
                                    break;
                                
                                case 'E':
                                    if (!east)
                                    {
                                        moves.Add((x, y), (x + 1, y));
                                        i = 5;
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        mapNext[y][x] = true;
                    }
                }
            }
            directions.Add(directions[0]);
            directions.RemoveAt(0);

            var hasMoved = false;
            foreach (var group in moves.GroupBy(kvp => kvp.Value))
            {
                if (group.Count() == 1)
                {
                    var pos = group.Single().Value;
                    mapNext[pos.Item2][pos.Item1] = true;
                    hasMoved = true;
                }
                else
                {
                    foreach (var pos in group.Select(i => i.Key))
                    {
                        mapNext[pos.Item2][pos.Item1] = true;
                    }
                }
            }

            (map, mapNext) = (mapNext, map);
            foreach (var row in mapNext)
                for (var x = 0; x < row.Length; x++)
                    row[x] = false;
            if (map.Length < 30)
            {
                Console.WriteLine();
                Console.WriteLine("Round " + round);
                foreach (var r in map)
                    Console.WriteLine(string.Join("", r.Select(c => c ? '#' : '.')));
            }

            if (!hasMoved)
            {
                return Task.FromResult(round.ToString());
            }
        }
    }

    public string? TestInput => @"....#..
..###.#
#...#.#
.#...##
#.###..
##.#.##
.#..#..";
}