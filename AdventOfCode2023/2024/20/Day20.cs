namespace AdventOfCode2023._2024._20;

public class Day20 : IDay
{
    public int Year => 2024;

    public int Day => 20;

    public string? Part1TestSolution => "5";

    public Task<string> Part1(string input)
    {

        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var mapScore = Enumerable.Range(0, map.Length)
            .Select(_ => Enumerable.Range(0, map[0].Length).Select(_ => 
                (Score: int.MaxValue, Pathway: new List<(int X, int Y)>())).ToArray()).ToArray();
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
                var newScore = checking.Score + 1;
                if (map[checking.Y + direction.Y][checking.X + direction.X] != '#' &&
                    mapScore[checking.Y + direction.Y][checking.X + direction.X].Score >= newScore)
                {
                    if (mapScore[checking.Y + direction.Y][checking.X + direction.X].Score == newScore)
                    {
                        mapScore[checking.Y + direction.Y][checking.X + direction.X].Pathway =
                            [
                                .. mapScore[checking.Y + direction.Y][checking.X + direction.X].Pathway,
                                ..checking.Pathway, (checking.X, checking.Y)
                            ];
                        mapScore[checking.Y + direction.Y][checking.X + direction.X].Pathway =
                            mapScore[checking.Y + direction.Y][checking.X + direction.X].Pathway
                                .Distinct()
                                .ToList();
                    }
                    else
                    {
                        mapScore[checking.Y + direction.Y][checking.X + direction.X].Pathway = [.. checking.Pathway, (checking.X, checking.Y)];
                    }

                    mapScore[checking.Y + direction.Y][checking.X + direction.X].Score = newScore;
                    toCheck.Enqueue((checking.X + direction.X, checking.Y + direction.Y, checkDirection, newScore,
                        mapScore[checking.Y + direction.Y][checking.X + direction.X].Pathway));
                }
            }
        }

        var count = 0;
        var cheats = new[] { (X: 2, Y: 0), (X: 1, Y: 1), (X: 0, Y: 2), (X: -1, Y: 1), (X: -2, Y: 0), (X: -1, Y: -1), (X: 0, Y: -2), (X: 1, Y: -1) };
        var minScore = mapScore[endY][endX].Score;
        var bestPathways = mapScore[endY][endX].Pathway.Distinct().Union(new []{(X:endX, Y:endY)})
            .ToArray();
        foreach(var p in bestPathways){
            foreach (var cheat in cheats)
            {
                var compareX = p.X + cheat.X;
                var compareY = p.Y + cheat.Y;
                if (compareY < 0 || compareX < 0 || compareY >= map.Length || compareX >= map[compareY].Length)
                    continue;
                if (!bestPathways.Contains((compareX, compareY)) || map[compareY][compareX] == '#')
                {
                    continue;
                }
                else
                {
                    var diff = mapScore[compareY][compareX].Score - mapScore[p.Y][p.X].Score;

                    if (diff >= (map.Length > 15 ? 102 : 22))
                    {
                        count++;
                    }
                }
            }
        }

        return Task.FromResult(count.ToString());
    }

    public string? Part2TestSolution => "285";


    public Task<string> Part2(string input)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var mapScore = Enumerable.Range(0, map.Length)
            .Select(_ => Enumerable.Range(0, map[0].Length).Select(_ =>
                (Score: int.MaxValue, Pathway: new List<(int X, int Y)>())).ToArray()).ToArray();
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
                var newScore = checking.Score + 1;
                if (map[checking.Y + direction.Y][checking.X + direction.X] != '#' &&
                    mapScore[checking.Y + direction.Y][checking.X + direction.X].Score >= newScore)
                {
                    if (mapScore[checking.Y + direction.Y][checking.X + direction.X].Score == newScore)
                    {
                        mapScore[checking.Y + direction.Y][checking.X + direction.X].Pathway =
                            [
                                .. mapScore[checking.Y + direction.Y][checking.X + direction.X].Pathway,
                                ..checking.Pathway, (checking.X, checking.Y)
                            ];
                        mapScore[checking.Y + direction.Y][checking.X + direction.X].Pathway =
                            mapScore[checking.Y + direction.Y][checking.X + direction.X].Pathway
                                .Distinct()
                                .ToList();
                    }
                    else
                    {
                        mapScore[checking.Y + direction.Y][checking.X + direction.X].Pathway = [.. checking.Pathway, (checking.X, checking.Y)];
                    }

                    mapScore[checking.Y + direction.Y][checking.X + direction.X].Score = newScore;
                    toCheck.Enqueue((checking.X + direction.X, checking.Y + direction.Y, checkDirection, newScore,
                        mapScore[checking.Y + direction.Y][checking.X + direction.X].Pathway));
                }
            }
        }

        var count = 0;
        var cheats = new List<(int X, int Y)>();
        for (var i = -20; i <= 20; i++)
        {
            for (var j = -20; j <= 20; j++)
            {
                if (Math.Abs(j) + Math.Abs(i) is <= 20 and > 0)
                {
                    cheats.Add((i, j));
                }
            }
        }

        var minScore = mapScore[endY][endX].Score;
        var bestPathways = mapScore[endY][endX].Pathway.Distinct().Union(new[] { (X: endX, Y: endY) })
            .ToArray();
        foreach (var p in bestPathways)
        {
            foreach (var cheat in cheats)
            {
                var compareX = p.X + cheat.X;
                var compareY = p.Y + cheat.Y;
                if (compareY < 0 || compareX < 0 || compareY >= map.Length || compareX >= map[compareY].Length)
                    continue;
                if (!bestPathways.Contains((compareX, compareY)) || map[compareY][compareX] == '#')
                {
                    continue;
                }
                else
                {
                    var diff = mapScore[compareY][compareX].Score - mapScore[p.Y][p.X].Score;

                    if (diff >= (map.Length > 15 ? 100 : 50) + Math.Abs(cheat.X) + Math.Abs(cheat.Y))
                    {
                        count++;
                    }
                }
            }
        }

        return Task.FromResult(count.ToString());
    }

    public string? TestInput => @"###############
#...#...#.....#
#.#.#.#.#.###.#
#S#...#.#.#...#
#######.#.#.###
#######.#.#...#
#######.#.###.#
###..E#...#...#
###.#######.###
#...###...#...#
#.#####.#.###.#
#.#...#.#.#...#
#.#.#.#.#.#.###
#...#...#...###
###############";
}