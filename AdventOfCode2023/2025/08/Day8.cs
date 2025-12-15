namespace AdventOfCode2023._2025._08;

internal class Day8 : IDay
{
    public int Year => 2025;

    public int Day => 8;

    public string? Part1TestSolution => "40";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select((l, i) => (l.Split(",").Select(long.Parse).ToArray(), Circuit:i))
            .ToArray();


        var distances = new List<(int indexA, int indexB, double distance)>();
        for (var i = 0; i < inputs.Length; i++)
        {
            var (junctionA, indexA) = inputs[i];
            for (var j = i + 1; j < inputs.Length; j++)
            {
                var (junctionB, indexB) = inputs[j];
                var distance = Math.Pow(junctionA[0] - junctionB[0], 2) +
                               Math.Pow(junctionA[1] - junctionB[1], 2) +
                               Math.Pow(junctionA[2] - junctionB[2], 2);
                distances.Add((indexA, indexB, distance));
            }
        }

        distances = distances.OrderBy(d => d.distance).ToList();

        for (var n = 0; n < (inputs.Length > 20 ? 1000 : 10); n++)
        {
            var (indexA, indexB, _) = distances[n];
            var circuitA = inputs[indexA].Circuit;
            for (var i = 0; i < inputs.Length; i++)
            {
                if (inputs[i].Circuit == circuitA)
                {
                    inputs[i].Circuit = inputs[indexB].Circuit;
                }
            }
        }

        var circuitSizes = inputs.Select(i => i.Circuit).GroupBy(c => c).Select(c => c.Count()).OrderByDescending(c => c).ToArray();
        Console.WriteLine(string.Join(", ", circuitSizes));
        return Task.FromResult(circuitSizes.Take(3).Aggregate(1, (acc, val) => acc * val).ToString());
    }

    public string? Part2TestSolution => "25272";

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select((l, i) => (l.Split(",").Select(long.Parse).ToArray(), Circuit: i))
            .ToArray();


        var distances = new List<(int indexA, int indexB, double distance)>();
        for (var i = 0; i < inputs.Length; i++)
        {
            var (junctionA, indexA) = inputs[i];
            for (var j = i + 1; j < inputs.Length; j++)
            {
                var (junctionB, indexB) = inputs[j];
                var distance = Math.Pow(junctionA[0] - junctionB[0], 2) +
                               Math.Pow(junctionA[1] - junctionB[1], 2) +
                               Math.Pow(junctionA[2] - junctionB[2], 2);
                distances.Add((indexA, indexB, distance));
            }
        }

        distances = distances.OrderBy(d => d.distance).ToList();

        for (var n = 0; n < distances.Count; n++)
        {
            var (indexA, indexB, _) = distances[n];
            var circuitA = inputs[indexA].Circuit;
            for (var i = 0; i < inputs.Length; i++)
            {
                if (inputs[i].Circuit == circuitA)
                {
                    inputs[i].Circuit = inputs[indexB].Circuit;
                }
            }

            if (inputs.Select(i => i.Circuit).Distinct().Count() == 1)
            {
                return Task.FromResult((inputs[indexA].Item1[0] * inputs[indexB].Item1[0]).ToString());
            }
        }

        throw new NotImplementedException();
    }

    public string? TestInput => "162,817,812\r\n57,618,57\r\n906,360,560\r\n592,479,940\r\n352,342,300\r\n466,668,158\r\n542,29,236\r\n431,825,988\r\n739,650,466\r\n52,470,668\r\n216,146,977\r\n819,987,18\r\n117,168,530\r\n805,96,715\r\n346,949,466\r\n970,615,88\r\n941,993,340\r\n862,61,35\r\n984,92,344\r\n425,690,689";
}