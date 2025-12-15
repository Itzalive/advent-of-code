namespace AdventOfCode2023._2025._09;

internal class Day9 : IDay
{
    public int Year => 2025;

    public int Day => 9;

    public string? Part1TestSolution => "50";

    public Task<string> Part1(string input)
    {
        var inputs = input.Split(Environment.NewLine).Select(l => l.Split(",").Select(long.Parse).ToArray()).ToArray();
        var maxArea = 0L;
        for (var i = 0; i < inputs.Length; i++)
        {
            for (var j = i + 1; j < inputs.Length; j++)
            {
                var area = (Math.Abs(inputs[i][0] - inputs[j][0]) +1) * (Math.Abs(inputs[i][1] - inputs[j][1]) + 1);
                if (area > maxArea)
                {
                    maxArea = area;
                }
            }
        }
        return Task.FromResult(maxArea.ToString());
    }

    public string? Part2TestSolution => "24";

    public Task<string> Part2(string input)
    {
        var inputs = input.Split(Environment.NewLine).ToArray();
        return Task.FromResult("");
    }

    public string? TestInput => "7,1\r\n11,1\r\n11,7\r\n9,7\r\n9,5\r\n2,5\r\n2,3\r\n7,3";


}