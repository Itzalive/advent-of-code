namespace AdventOfCode2023._2024._18;

public class Day18 : IDay
{
    public int Year => 2024;

    public int Day => 18;

    public string? Part1TestSolution => "22";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var map = Enumerable.Range(0, inputs.Length > 50 ? 71 : 7)
            .Select(_ => Enumerable.Range(0, inputs.Length > 50 ? 71 : 7).Select(_ => '.').ToArray()).ToArray();
        var mapScore = Enumerable.Range(0, map.Length)
            .Select(_ => Enumerable.Range(0, map[0].Length).Select(_ => int.MaxValue).ToArray()).ToArray();
        var posY = 0;
        var posX = 0;
        var endY = map.Length - 1;
        var endX = map.Length - 1;
        foreach (var t in inputs)
        {
            var point = t.Split(',').Select(int.Parse).ToArray();
            map[point[1]][point[0]] = '#';
            mapScore = Enumerable.Range(0, map.Length)
                .Select(_ => Enumerable.Range(0, map[0].Length).Select(_ => int.MaxValue).ToArray()).ToArray();

            var directions = new[] { (X: 1, Y: 0), (X: 0, Y: -1), (X: -1, Y: 0), (X: 0, Y: 1) };
            var toCheck = new Queue<(int X, int Y, int Score)>();
            toCheck.Enqueue((posX, posY, 0));
            while (toCheck.Any())
            {
                var checking = toCheck.Dequeue();
                for (var i = 0; i < 4; i++)
                {
                    var direction = directions[i];
                    var newScore = checking.Score + 1;
                    if(checking.Y + direction.Y < 0 || checking.Y + direction.Y >= mapScore.Length || checking.X + direction.X < 0 || checking.X + direction.X >= mapScore[0].Length)
                        continue;
                    if (map[checking.Y + direction.Y][checking.X + direction.X] != '#' && mapScore[checking.Y + direction.Y][checking.X + direction.X] > newScore)
                    {
                        mapScore[checking.Y + direction.Y][checking.X + direction.X] = newScore;
                        toCheck.Enqueue((checking.X + direction.X, checking.Y + direction.Y, newScore));
                    }
                }
            }

            if (mapScore[endY][endX] == int.MaxValue)
            {
                return Task.FromResult(t);
            }
        }

        //for (var y = 0; y < map.Length; y++)
        //{
        //    for (var x = 0; x < map[y].Length; x++)
        //    {
        //        if (map[y][x] == '#')
        //        {
        //            Console.Write('#');
        //        }
        //        else
        //        {
        //            var minScore = mapScore[y][x].Min();
        //            if (minScore == int.MaxValue)
        //            {
        //                Console.Write('.');
        //            }
        //            else
        //            {
        //                if (mapScore[y][x][0] == minScore)
        //                {
        //                    Console.Write('>');
        //                }
        //                else if (mapScore[y][x][1] == minScore)
        //                {
        //                    Console.Write('^');
        //                }
        //                else if (mapScore[y][x][2] == minScore)
        //                {
        //                    Console.Write('<');
        //                }
        //                else if (mapScore[y][x][3] == minScore)
        //                {
        //                    Console.Write('v');
        //                }
        //            }
        //        }
        //    }
        //    Console.WriteLine();
        //}

        return Task.FromResult(mapScore[endY][endX].ToString());
    }

    public string? Part2TestSolution => "6,1";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        var map = Enumerable.Range(0, inputs.Length > 50 ? 71 : 7)
            .Select(_ => Enumerable.Range(0, inputs.Length > 50 ? 71 : 7).Select(_ => '.').ToArray()).ToArray();
        var mapScore = Enumerable.Range(0, map.Length)
            .Select(_ => Enumerable.Range(0, map[0].Length).Select(_ => int.MaxValue).ToArray()).ToArray();
        var posY = 0;
        var posX = 0;
        var endY = map.Length - 1;
        var endX = map.Length - 1;
        foreach (var t in inputs)
        {
            var point = t.Split(',').Select(int.Parse).ToArray();
            map[point[1]][point[0]] = '#';
            mapScore = Enumerable.Range(0, map.Length)
                .Select(_ => Enumerable.Range(0, map[0].Length).Select(_ => int.MaxValue).ToArray()).ToArray();

            var directions = new[] { (X: 1, Y: 0), (X: 0, Y: -1), (X: -1, Y: 0), (X: 0, Y: 1) };
            var toCheck = new Queue<(int X, int Y, int Score)>();
            toCheck.Enqueue((posX, posY, 0));
            while (toCheck.Any())
            {
                var checking = toCheck.Dequeue();
                for (var i = 0; i < 4; i++)
                {
                    var direction = directions[i];
                    var newScore = checking.Score + 1;
                    if (checking.Y + direction.Y < 0 || checking.Y + direction.Y >= mapScore.Length || checking.X + direction.X < 0 || checking.X + direction.X >= mapScore[0].Length)
                        continue;
                    if (map[checking.Y + direction.Y][checking.X + direction.X] != '#' && mapScore[checking.Y + direction.Y][checking.X + direction.X] > newScore)
                    {
                        mapScore[checking.Y + direction.Y][checking.X + direction.X] = newScore;
                        toCheck.Enqueue((checking.X + direction.X, checking.Y + direction.Y, newScore));
                    }
                }
            }

            if (mapScore[endY][endX] == int.MaxValue)
            {
                return Task.FromResult(t);
            }
        }

        return Task.FromResult("Nope");
    }

    public string? TestInput => "5,4\r\n4,2\r\n4,5\r\n3,0\r\n2,1\r\n6,3\r\n2,4\r\n1,5\r\n0,6\r\n3,3\r\n2,6\r\n5,1\r\n1,2\r\n5,5\r\n2,5\r\n6,5\r\n1,4\r\n0,4\r\n6,4\r\n1,1\r\n6,1\r\n1,0\r\n0,5\r\n1,6\r\n2,0";
}