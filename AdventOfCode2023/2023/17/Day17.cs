namespace AdventOfCode2023._2023._17;

public class Day17 : IDay
{
    public int Year => 2023;

    public int Day => 17;

    public string? Part1TestSolution => "102";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine)
            .Select(l => l.ToCharArray().Select(c => new Node{ Cost = int.Parse(new string(c, 1)) }).ToArray()).ToArray();
        var lestPossibleSolutions = new List<PossibleSolution>()
        {
            new()
            {
                Score = 0, Position = new[] { 0, 0 }, HeadingIndex = 0, NumInHeading = 0
            }
        };

        inputs[0][0].Solutions[0][0] = lestPossibleSolutions[0];

        while (true)
        {
            var nextToTest = lestPossibleSolutions.OrderBy(p => p.Score).First();
            lestPossibleSolutions.Remove(nextToTest);

            for (var n = -1; n <= 1; n++)
            {
                var headingIndex = (nextToTest.HeadingIndex + n + 4) % 4;
                var heading = Headings[headingIndex];
                var numOnHeading = n == 0 ? nextToTest.NumInHeading + 1 : 0;
                if (numOnHeading >= 3) continue;
                var newPosition = new[] { nextToTest.Position[0] + heading[0], nextToTest.Position[1] + heading[1] };
                if (newPosition[0] < 0 || newPosition[1] < 0 || newPosition[0] >= inputs.Length ||
                    newPosition[1] >= inputs[0].Length)
                    continue;

                var node = inputs[newPosition[0]][newPosition[1]];
                var score = nextToTest.Score + node.Cost;
                if (newPosition[0] == inputs.Length - 1 && newPosition[1] == inputs[0].Length - 1)
                {
                    return Task.FromResult(score.ToString());
                }

                if (node.Solutions[headingIndex][numOnHeading] != null)
                    continue;

                var possibleSolution = new PossibleSolution
                {
                    Score = score,
                    Position = newPosition,
                    HeadingIndex = headingIndex,
                    NumInHeading = numOnHeading
                };
                node.Solutions[headingIndex][numOnHeading] = possibleSolution;
                lestPossibleSolutions.Add(possibleSolution);
            }
        }


        return Task.FromResult("");
    }

    public class Node
    {
        public int Cost { get; set; }

        public PossibleSolution?[][] Solutions { get; set; } = new PossibleSolution?[4][]
        {
            new PossibleSolution?[10],
            new PossibleSolution?[10],
            new PossibleSolution?[10],
            new PossibleSolution?[10]
        };
    }

    public record PossibleSolution
    {
        public int Score { get; set; }

        public int[] Position { get; set; }

        public int HeadingIndex { get; set; }

        public int NumInHeading { get; set; }
    }

    public int[][] Headings => new[]
    {
        new int[] { 0, 1 },
        new int[] { 1, 0 },
        new int[] { 0, -1 },
        new int[] { -1, 0 }
    };

    public string? Part2TestSolution => "94";


    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine)
            .Select(l => l.ToCharArray().Select(c => new Node { Cost = int.Parse(new string(c, 1)) }).ToArray())
            .ToArray();
        var lestPossibleSolutions = new List<PossibleSolution>()
        {
            new()
            {
                Score = 0, Position = new[] { 0, 0 }, HeadingIndex = 0, NumInHeading = 0
            }
        };

        inputs[0][0].Solutions[0][0] = lestPossibleSolutions[0];

        while (true)
        {
            var nextToTest = lestPossibleSolutions.OrderBy(p => p.Score).First();
            lestPossibleSolutions.Remove(nextToTest);

            for (var n = -1; n <= 1; n++)
            {
                if(n != 0 && nextToTest.NumInHeading < 3) continue;

                var headingIndex = (nextToTest.HeadingIndex + n + 4) % 4;
                var heading = Headings[headingIndex];
                var numOnHeading = n == 0 ? nextToTest.NumInHeading + 1 : 0;
                if (numOnHeading >= 10) continue;
                var newPosition = new[] { nextToTest.Position[0] + heading[0], nextToTest.Position[1] + heading[1] };
                if (newPosition[0] < 0 || newPosition[1] < 0 || newPosition[0] >= inputs.Length ||
                    newPosition[1] >= inputs[0].Length)
                    continue;

                var node = inputs[newPosition[0]][newPosition[1]];
                var score = nextToTest.Score + node.Cost;
                if (newPosition[0] == inputs.Length - 1 && newPosition[1] == inputs[0].Length - 1)
                {
                    return Task.FromResult(score.ToString());
                }

                if (node.Solutions[headingIndex][numOnHeading] != null)
                    continue;

                var possibleSolution = new PossibleSolution
                {
                    Score = score,
                    Position = newPosition,
                    HeadingIndex = headingIndex,
                    NumInHeading = numOnHeading
                };
                node.Solutions[headingIndex][numOnHeading] = possibleSolution;
                lestPossibleSolutions.Add(possibleSolution);
            }
        }


        return Task.FromResult("");
    }

    public string? TestInput => @"2413432311323
3215453535623
3255245654254
3446585845452
4546657867536
1438598798454
4457876987766
3637877979653
4654967986887
4564679986453
1224686865563
2546548887735
4322674655533";
}