namespace AdventOfCode2023._2025._05;

internal class Day5 : IDay
{
    public int Year => 2025;

    public int Day => 5;

    public string? Part1TestSolution => "3";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var ranges = inputs[0].Split(Environment.NewLine).Select(l => l.Split("-").Select(long.Parse).ToArray()).ToArray();
        var ingredients = inputs[1].Split(Environment.NewLine).Select(long.Parse).ToArray();

        var freshIngredients = ingredients.Where(i => ranges.Any(r => r[0] <= i && i <= r[1]));
        return Task.FromResult(freshIngredients.Count().ToString());
    }

    public string? Part2TestSolution => "14";

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine + Environment.NewLine).ToArray();
        var ranges = inputs[0].Split(Environment.NewLine).Select(l => l.Split("-").Select(long.Parse).ToArray()).ToArray();
        var ingredientsCount = 0L;
        var rangesAdded = new List<long[]>();
        for (var i = 0; i< ranges.Length; i++)
        {
            for (var j = 0; j < rangesAdded.Count; j++)
            {
                var range = rangesAdded[j];
                if (range[0] >= ranges[i][0] && range[1] <= ranges[i][1])
                {
                    Console.WriteLine($"Range removed: {range[0]}-{range[1]}");
                    ingredientsCount -= range[1] - range[0] + 1;
                    rangesAdded.Remove(range);
                    j--;
                    continue;
                }

                if (range[0] < ranges[i][0])
                {
                    ranges[i][0] = Math.Max(ranges[i][0], range[1] + 1);
                }
                
                if (range[1] > ranges[i][1])
                {
                    ranges[i][1] = Math.Min(ranges[i][1], range[0] - 1);
                }
            }
            var count = Math.Max(0, ranges[i][1] - ranges[i][0] + 1);
            if (count > 0)
            {
                ingredientsCount += count;
                rangesAdded.Add(ranges[i]);
                Console.WriteLine($"Range added: {ranges[i][0]}-{ranges[i][1]}");
            }
        }
        return Task.FromResult(ingredientsCount.ToString());
    }

    public string? TestInput => "3-5\r\n10-14\r\n16-20\r\n12-18\r\n19-20\r\n\r\n1\r\n5\r\n8\r\n11\r\n17\r\n32";


}