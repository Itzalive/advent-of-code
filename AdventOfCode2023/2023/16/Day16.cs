namespace AdventOfCode2023._2023._16;

public class Day16 : IDay
{
    public int Year => 2023;

    public int Day => 16;

    public string? Part1TestSolution => "46";

    public Task<string> Part1(string input)
    {
        return Task.FromResult(Score(input, (coords: new []{0,0}, heading: new[]{0, 1})).ToString());
    }

    private static int Score(string input, (int[] coords, int[] heading) valueTuple)
    {
        var map = input.Split(Environment.NewLine).Select(l => l.ToCharArray().Select(c => new Tile{Character = c}).ToArray()).ToArray();

        var queueToAnalyse = new Queue<(int[] coords, int[] heading)>();
        queueToAnalyse.Enqueue(valueTuple);

        while (queueToAnalyse.Count > 0)
        {
            var toAnalyse = queueToAnalyse.Dequeue();
            if (toAnalyse.coords[0] < 0 || toAnalyse.coords[1] < 0 || toAnalyse.coords[0] >= map.Length ||
                toAnalyse.coords[1] >= map[0].Length)
            {
                continue;
            }

            var tile = map[toAnalyse.coords[0]][toAnalyse.coords[1]];
            if (tile.IsEnergized && tile.IsChecked[toAnalyse.heading[0] + 1, toAnalyse.heading[1]+ 1])
            {
                continue;
            }

            tile.IsEnergized = true;
            tile.IsChecked[toAnalyse.heading[0] + 1, toAnalyse.heading[1]+ 1] = true;
            switch (tile.Character)
            {
                case '.':
                    queueToAnalyse.Enqueue((
                        new[]
                        {
                            toAnalyse.coords[0] + toAnalyse.heading[0], toAnalyse.coords[1] + toAnalyse.heading[1]
                        }, toAnalyse.heading));
                    break;
                case '\\':
                    var newHeading = new[] { toAnalyse.heading[1], toAnalyse.heading[0] };
                    queueToAnalyse.Enqueue((
                        new[]
                        {
                            toAnalyse.coords[0] + newHeading[0], toAnalyse.coords[1] + newHeading[1]
                        }, newHeading));
                    break;
                
                case '/':
                    var newHeadingM = new[] { -toAnalyse.heading[1], -toAnalyse.heading[0] };
                    queueToAnalyse.Enqueue((
                        new[]
                        {
                            toAnalyse.coords[0] + newHeadingM[0], toAnalyse.coords[1] + newHeadingM[1]
                        }, newHeadingM));
                    break;
                case '-':
                    queueToAnalyse.Enqueue((
                        new[]
                        {
                            toAnalyse.coords[0], toAnalyse.coords[1] + 1
                        }, new[] { 0, 1 }));

                    queueToAnalyse.Enqueue((
                        new[]
                        {
                            toAnalyse.coords[0], toAnalyse.coords[1] - 1
                        }, new[] { 0, -1 }));
                    break;
                
                case '|':
                    queueToAnalyse.Enqueue((
                        new[]
                        {
                            toAnalyse.coords[0] + 1, toAnalyse.coords[1]
                        }, new[] { 1, 0 }));

                    queueToAnalyse.Enqueue((
                        new[]
                        {
                            toAnalyse.coords[0] - 1, toAnalyse.coords[1]
                        }, new[] { -1, 0 }));
                    break;
                default:
                    throw new NotImplementedException();

            }
        }

        return map.Sum(l => l.Count(l=>l.IsEnergized));
    }

    public string? Part2TestSolution => "51";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.ToCharArray()).ToArray();
        var scores = new List<int>();
        for (var y = 0; y < inputs.Length; y++)
        {
            scores.Add(Score(input, (coords: new[] { y, 0 }, heading: new[] { 0, 1 })));

            scores.Add(Score(input, (coords: new[] { y, inputs[0].Length - 1 }, heading: new[] { 0, -1 })));
        }
        for (var x = 0; x < inputs[0].Length; x++)
        {
            scores.Add(Score(input, (coords: new[] { 0, x }, heading: new[] { 1, 0 })));

            scores.Add(Score(input, (coords: new[] { inputs.Length - 1, x }, heading: new[] { -1, 0 })));
        }
        return Task.FromResult(scores.Max().ToString());
    }

    private class Tile
    {
        public char Character { get; set; }

        public bool IsEnergized { get; set; }

        public bool[,] IsChecked { get; set; } = new bool[3, 3];
    }

    public string? TestInput => @".|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....";
}